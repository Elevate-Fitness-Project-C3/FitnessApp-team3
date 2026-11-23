using System.Security.Cryptography;

namespace IdentityService.Infrastructure.Settings;

public class JwtSettings
{
    public const string SectionName = "JwtSettings";
    public string Issuer { get; set; } = string.Empty;
    public string Audience { get; set; } = string.Empty;
    public string KeyId { get; set; } = string.Empty;
    public string PrivateKeyPem { get; set; } = string.Empty;
    public int AccessLifetimeMinutes { get; set; } = 15;
    public int RefreshLifetimeDays { get; set; } = 7;

    public RSA LoadSigningKey()
    {
        if (string.IsNullOrWhiteSpace(Issuer) || string.IsNullOrWhiteSpace(Audience)
            || string.IsNullOrWhiteSpace(KeyId) || AccessLifetimeMinutes <= 0 || RefreshLifetimeDays <= 0)
            throw new InvalidOperationException("JWT issuer, audience, key ID and positive token lifetimes are required.");

        var rsa = RSA.Create();
        try
        {
            rsa.ImportFromPem(PrivateKeyPem);
            if (rsa.KeySize < 2048 || rsa.ExportParameters(true).D is null)
                throw new CryptographicException();
            return rsa;
        }
        catch (Exception error) when (error is ArgumentException or CryptographicException)
        {
            rsa.Dispose();
            throw new InvalidOperationException("JwtSettings:PrivateKeyPem requires an RSA private key of at least 2048 bits.");
        }
    }
}
