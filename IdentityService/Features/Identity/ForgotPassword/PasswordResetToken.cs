using System.Security.Cryptography;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Services;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Features.Identity.ForgotPassword;

internal static class PasswordResetToken
{
    public static string Create(Guid challengeId)
        => $"{challengeId:N}.{Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32))}";

    public static bool TryGetChallengeId(string? token, out Guid challengeId)
    {
        challengeId = default;
        return token is { Length: 76 } && token[32] == '.'
            && Guid.TryParseExact(token.AsSpan(0, 32), "N", out challengeId);
    }

    public static string Hash(IOtpService otpService, string token)
        => otpService.HashCode($"{OtpCode.PasswordResetTokenPurpose}:{token}");
}
