using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Services;
using IdentityService.Infrastructure.Settings;
using Microsoft.Extensions.Options;

namespace IdentityService.Infrastructure.Services;

public class OtpService(IOptions<OtpSettings> options) : IOtpService
{
    public string GenerateCode()
    {
        var length = options.Value.Length;
        var maximum = (int)Math.Pow(10, length);
        return RandomNumberGenerator.GetInt32(maximum).ToString($"D{length}", CultureInfo.InvariantCulture);
    }

    public string HashCode(string code)
    {
        var key = Convert.FromBase64String(options.Value.ProtectionKey);
        try
        {
            return Convert.ToHexString(HMACSHA256.HashData(key, Encoding.UTF8.GetBytes(code)));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }
}
