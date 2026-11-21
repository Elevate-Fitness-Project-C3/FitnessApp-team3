namespace IdentityService.Features.Identity.Login.DTOs;

public class LoginResult
{
    public required LoginResponseDto Response { get; init; }
    public required string RefreshToken { get; init; }
    public DateTimeOffset RefreshExpiresAt { get; init; }
}
