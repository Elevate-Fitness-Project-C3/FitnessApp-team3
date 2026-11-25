using System.Security.Cryptography;
using System.Text;
using IdentityService.Domain.Entities.Identity;
using Microsoft.IdentityModel.Tokens;

namespace IdentityService.Common.Identity;

internal static class RefreshTokenValue
{
    public static (string Token, RefreshToken Record) Create(int userId, DateTimeOffset expiresAt, int? sessionRootTokenId = null)
    {
        var token = Base64UrlEncoder.Encode(RandomNumberGenerator.GetBytes(32));
        return (token, new RefreshToken
        {
            UserId = userId,
            SessionRootTokenId = sessionRootTokenId,
            TokenHash = Hash(token),
            ExpiresAt = expiresAt
        });
    }

    public static bool IsValidFormat(string? token)
        => token is { Length: 43 } && token.All(character => char.IsAsciiLetterOrDigit(character) || character is '-' or '_');

    public static string Hash(string token)
        => Convert.ToHexString(SHA256.HashData(Encoding.UTF8.GetBytes(token)));
}
