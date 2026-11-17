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

    public string HashCode(Guid challengeId, string purpose, string code)
    {
        var key = Convert.FromBase64String(options.Value.ProtectionKey);
        try
        {
            var payload = Encoding.UTF8.GetBytes($"FitnessApp|{purpose.Length}:{purpose}|{challengeId:N}|{code}");
            return Convert.ToHexString(HMACSHA256.HashData(key, payload));
        }
        finally
        {
            CryptographicOperations.ZeroMemory(key);
        }
    }

    public bool VerifyCode(Guid challengeId, string purpose, string code, string codeHash)
    {
        if (code.Length != options.Value.Length || code.Any(character => character is < '0' or > '9'))
            return false;

        try
        {
            var expected = Convert.FromHexString(codeHash);
            var actual = Convert.FromHexString(HashCode(challengeId, purpose, code));
            return expected.Length == actual.Length
                && CryptographicOperations.FixedTimeEquals(expected, actual);
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
