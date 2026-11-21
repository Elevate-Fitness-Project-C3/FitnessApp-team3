using IdentityService.Domain.Entities.Identity;

namespace IdentityService.Features.Identity.Register.DTOs;

public class GeneratedEmailVerificationOtp(OtpCode challenge, string code)
{
    public OtpCode Challenge { get; } = challenge;
    public string Code { get; } = code;
}
