namespace IdentityService.Domain.Services;

public interface IOtpService
{
    string GenerateCode();
    string HashCode(Guid challengeId, string purpose, string code);
    bool VerifyCode(Guid challengeId, string purpose, string code, string codeHash);
}
