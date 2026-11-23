using Carter;
using IdentityService.Infrastructure.Settings;
using Microsoft.IdentityModel.Tokens;
using IdentityService.Common.Middleware;
using IdentityService.Infrastructure;
using IdentityService.Features.Shared;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Threading.RateLimiting;

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
    options.AddPolicy("login", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 20,
                Window = TimeSpan.FromMinutes(1),
                QueueLimit = 0
            }));

    options.AddPolicy("registration", context =>
        RateLimitPartition.GetFixedWindowLimiter(
            context.Connection.RemoteIpAddress?.ToString() ?? "unknown",
            _ => new FixedWindowRateLimiterOptions
            {
                PermitLimit = 5,
                Window = TimeSpan.FromMinutes(15),
                QueueLimit = 0
            }));

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

public partial class Program { }
