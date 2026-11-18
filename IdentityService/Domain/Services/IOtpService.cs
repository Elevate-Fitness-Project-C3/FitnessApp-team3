namespace IdentityService.Domain.Services;

public interface IOtpService
{
    string GenerateCode();
    string HashCode(string code);
}
