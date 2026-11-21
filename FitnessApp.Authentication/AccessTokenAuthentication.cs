using System.Security.Cryptography;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;

namespace FitnessApp.Authentication;

public static class AccessTokenAuthentication
{
    public static void AddAccessTokenAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        var jwt = configuration.GetRequiredSection("JwtSettings");
        var issuer = RequiredSetting(jwt, "Issuer");
        var audience = RequiredSetting(jwt, "Audience");
        var pem = RequiredSetting(jwt, "PublicKeyPem").Trim();
        if (!pem.StartsWith("-----BEGIN PUBLIC KEY-----", StringComparison.Ordinal)
            || !pem.EndsWith("-----END PUBLIC KEY-----", StringComparison.Ordinal)
            || pem.Contains("PRIVATE KEY", StringComparison.Ordinal))
            throw new InvalidOperationException("JwtSettings:PublicKeyPem must contain a public PEM.");

        using var rsa = RSA.Create();
        rsa.ImportFromPem(pem);
        if (rsa.KeySize < 2048)
            throw new InvalidOperationException("The JWT verification key must be at least 2048 bits.");
        var key = new RsaSecurityKey(rsa.ExportParameters(false)) { KeyId = RequiredSetting(jwt, "KeyId") };

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
        {
            options.MapInboundClaims = false;
            options.IncludeErrorDetails = false;
            options.TokenValidationParameters = new TokenValidationParameters
            {
                RequireSignedTokens = true,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = key,
                TryAllIssuerSigningKeys = false,
                ValidAlgorithms = [SecurityAlgorithms.RsaSha256],
                ValidateIssuer = true,
                ValidIssuer = issuer,
                ValidateAudience = true,
                ValidAudience = audience,
                IgnoreTrailingSlashWhenValidatingAudience = false,
                RequireExpirationTime = true,
                ValidateLifetime = true,
                ClockSkew = TimeSpan.Zero,
                NameClaimType = "sub",
                RoleClaimType = "role"
            };
        });
        services.AddAuthorization();
    }

    private static string RequiredSetting(IConfigurationSection section, string name) =>
        !string.IsNullOrWhiteSpace(section[name]) ? section[name]! :
            throw new InvalidOperationException($"JwtSettings:{name} is required.");
}
