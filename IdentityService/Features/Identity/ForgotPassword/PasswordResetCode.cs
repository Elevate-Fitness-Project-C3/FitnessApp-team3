using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Services;

namespace IdentityService.Features.Identity.ForgotPassword;

internal static class PasswordResetCode
{
    public const int Length = 6;

    public static string Hash(IOtpService otpService, Guid challengeId, string code)
        => otpService.HashCode($"{OtpCode.PasswordResetPurpose}:{challengeId:N}:{code}");
}
