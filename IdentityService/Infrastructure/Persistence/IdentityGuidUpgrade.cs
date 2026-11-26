using System.Data;
using System.Text.Json.Nodes;
using IdentityService.Migrations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace IdentityService.Infrastructure.Persistence;

internal static class IdentityGuidUpgrade
{
    private const string UserCreatedType = "urn:message:IdentityService.Features.Identity.Register.Events:UserCreated";

    public static async Task ApplyAsync(string connectionString)
    {
        var options = new DbContextOptionsBuilder<AppDbContext>().UseSqlServer(connectionString).Options;
        await using var context = new AppDbContext(options);
        var applied = await context.Database.GetAppliedMigrationsAsync();
        if (applied.Any(migration => migration.EndsWith("_UseGuidIdentityKeys", StringComparison.Ordinal)))
        {
            await context.Database.MigrateAsync();
            return;
        }

        await context.Database.MigrateAsync(PrepareGuidIdentityKeys.Id);
        await BackfillAsync(connectionString);
        await context.Database.MigrateAsync();
    }

    private static async Task BackfillAsync(string connectionString)
    {
        var options = new DbContextOptionsBuilder().UseSqlServer(connectionString).UseModel(BuildModel()).Options;
        await using var context = new DbContext(options);
        await using var transaction = await context.Database.BeginTransactionAsync(IsolationLevel.Serializable);

        var users = await context.Set<LegacyUser>().ToListAsync();
        foreach (var user in users)
            user.GuidId ??= Guid.NewGuid();

        var userIds = users.ToDictionary(user => user.UserId, user => user.GuidId!.Value);
        var codes = await context.Set<LegacyOtp>().ToListAsync();
        foreach (var code in codes)
        {
            code.GuidId ??= Guid.NewGuid();
            code.GuidUserId = userIds[code.UserId];
        }

        var tokens = await context.Set<LegacyToken>().ToListAsync();
        foreach (var token in tokens)
        {
            token.GuidId ??= Guid.NewGuid();
            token.GuidUserId = userIds[token.UserId];
        }

        var tokenIds = tokens.ToDictionary(token => token.TokenId, token => token.GuidId!.Value);
        foreach (var token in tokens)
            token.GuidSessionRootTokenId = token.SessionRootTokenId is int rootId ? tokenIds[rootId] : null;

        var userCreatedMessages = await context.Set<LegacyOutboxMessage>()
            .Where(message => message.MessageType.Contains(UserCreatedType)).ToListAsync();
        foreach (var message in userCreatedMessages)
        {
            var envelope = JsonNode.Parse(message.Body);
            if (envelope?["message"] is not JsonObject payload || payload["userId"] is not JsonValue userId)
                throw new InvalidOperationException("An outbox UserCreated message has an unexpected payload.");

            if (userId.TryGetValue<int>(out var legacyUserId))
            {
                payload["userId"] = JsonValue.Create(userIds[legacyUserId]);
                message.Body = envelope.ToJsonString();
            }
            else if (!userId.TryGetValue<string>(out var id) || !Guid.TryParse(id, out _))
            {
                throw new InvalidOperationException("An outbox UserCreated message has an invalid user ID.");
            }
        }

        await context.SaveChangesAsync();
        await transaction.CommitAsync();
        Console.WriteLine($"GUIDs prepared for {users.Count} users, {codes.Count} challenges, and {tokens.Count} tokens.");
    }

    private static IModel BuildModel()
    {
        var model = SqlServerConventionSetBuilder.CreateModelBuilder();
        model.Entity<LegacyUser>().ToTable("ApplicationUser").HasKey(user => user.UserId);
        model.Entity<LegacyOtp>().ToTable("OtpCode").HasKey(code => code.OtpId);
        model.Entity<LegacyToken>().ToTable("RefreshToken").HasKey(token => token.TokenId);
        model.Entity<LegacyOutboxMessage>().ToTable("OutboxMessage").HasKey(message => message.SequenceNumber);
        return model.FinalizeModel();
    }

    private sealed class LegacyUser
    {
        public int UserId { get; set; }
        public Guid? GuidId { get; set; }
    }

    private sealed class LegacyOtp
    {
        public int OtpId { get; set; }
        public int UserId { get; set; }
        public Guid? GuidId { get; set; }
        public Guid? GuidUserId { get; set; }
    }

    private sealed class LegacyToken
    {
        public int TokenId { get; set; }
        public int UserId { get; set; }
        public int? SessionRootTokenId { get; set; }
        public Guid? GuidId { get; set; }
        public Guid? GuidUserId { get; set; }
        public Guid? GuidSessionRootTokenId { get; set; }
    }

    private sealed class LegacyOutboxMessage
    {
        public long SequenceNumber { get; set; }
        public required string MessageType { get; set; }
        public required string Body { get; set; }
    }
}
