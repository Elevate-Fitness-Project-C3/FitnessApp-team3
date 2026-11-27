namespace IdentityService.Features.Identity.RefreshTokens.DTOs;

public record RefreshTokenResult(string AccessToken, string RefreshToken, DateTimeOffset RefreshExpiresAt);
