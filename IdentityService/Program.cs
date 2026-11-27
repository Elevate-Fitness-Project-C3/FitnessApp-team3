using Carter;
using IdentityService.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Common.Middleware;
using IdentityService.Infrastructure;
using IdentityService.Infrastructure.Persistence;
using IdentityService.Features.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.RateLimiting;

if (args is ["--upgrade-identity-ids", ..])
{
    var configuration = WebApplication.CreateBuilder(args[1..]).Configuration;
    await IdentityGuidUpgrade.ApplyAsync(configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required."));
    return;
}

var builder = WebApplication.CreateBuilder(args);

if (args is ["--export-public-key", var path])
{
    var settings = builder.Configuration.GetRequiredSection(JwtSettings.SectionName).Get<JwtSettings>()!;
    using var key = settings.LoadSigningKey();
    using var output = new StreamWriter(new FileStream(path, FileMode.CreateNew, FileAccess.Write));
    output.WriteLine(key.ExportSubjectPublicKeyInfoPem());
    return;
}

builder.Services.AddCarter();
builder.Services.AddOpenApi();
builder.Services.AddIdentityRegistration(builder.Configuration);
builder.Services.Configure<RouteHandlerOptions>(options => options.ThrowOnBadRequest = true);
builder.Services.AddIdentityPersistence(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")
        ?? throw new InvalidOperationException("ConnectionStrings:DefaultConnection is required.")));
builder.Services.AddIdentityMessaging(builder.Configuration);
builder.Services.AddRateLimiter(options =>
{
    options.RejectionStatusCode = StatusCodes.Status429TooManyRequests;
    options.OnRejected = async (rejection, cancellationToken) =>
    {
        rejection.HttpContext.Response.StatusCode = StatusCodes.Status429TooManyRequests;
        if (rejection.Lease.TryGetMetadata(MetadataName.RetryAfter, out var retryAfter))
            rejection.HttpContext.Response.Headers.RetryAfter =
                Math.Ceiling(retryAfter.TotalSeconds).ToString(System.Globalization.CultureInfo.InvariantCulture);
        await rejection.HttpContext.Response.WriteAsJsonAsync(
            RequestResponse<object>.Fail("Too many requests. Try again later.", 429),
            cancellationToken);
    };
    options.AddPolicy("login", context => PerIpLimit(context, 20, TimeSpan.FromMinutes(1)));
    options.AddPolicy("registration", context => PerIpLimit(context, 5, TimeSpan.FromMinutes(15)));
    options.AddPolicy("password-reset-request", context => PerIpLimit(context, 5, TimeSpan.FromMinutes(15)));
    options.AddPolicy("password-reset-verify", context => PerIpLimit(context, 20, TimeSpan.FromMinutes(1)));
    options.AddPolicy("password-reset-complete", context => PerIpLimit(context, 20, TimeSpan.FromMinutes(1)));

    options.GlobalLimiter =
        PartitionedRateLimiter.Create<HttpContext, string>(context =>
        {
            var userId = context.User.Identity?.IsAuthenticated == true
                ? context.User.FindFirstValue(ClaimTypes.NameIdentifier)
                    ?? context.User.FindFirstValue("sub")
                : null;

            var partitionKey = !string.IsNullOrWhiteSpace(userId)
                ? $"user:{userId}"
                : $"ip:{context.Connection.RemoteIpAddress?.ToString() ?? "unknown"}";

            return RateLimitPartition.GetFixedWindowLimiter(
                partitionKey,
                _ => new FixedWindowRateLimiterOptions
                {
                    PermitLimit = 100,
                    Window = TimeSpan.FromMinutes(1),
                    QueueLimit = 0
                });
        });
});

var app = builder.Build();
app.Services.GetRequiredService<SigningCredentials>();

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseHttpsRedirection();
app.UseRouting();
app.UseRateLimiter();

if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwaggerUI(options =>
        options.SwaggerEndpoint("/openapi/v1.json", "Identity Service v1"));
}

app.MapCarter();

app.Run();

static RateLimitPartition<string> PerIpLimit(HttpContext context, int permitLimit, TimeSpan window)
    => RateLimitPartition.GetFixedWindowLimiter(
        context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
        _ => new FixedWindowRateLimiterOptions
        {
            PermitLimit = permitLimit,
            Window = window,
            QueueLimit = 0
        });

public partial class Program { }
