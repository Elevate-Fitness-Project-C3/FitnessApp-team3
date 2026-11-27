namespace IdentityService.Domain.Services;

public interface IOtpService
{
    string GenerateCode();
    string GenerateCode(int length);
    string HashCode(string code);
}
