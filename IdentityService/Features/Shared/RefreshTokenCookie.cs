namespace IdentityService.Features.Shared;

internal static class RefreshTokenCookie
{
    private const string Name = "refresh_token";

    public static string? Read(HttpRequest request) => request.Cookies[Name];

    public static void Write(HttpResponse response, string token, DateTimeOffset expiresAt)
    {
        var options = CreateOptions();
        options.Expires = expiresAt;
        response.Cookies.Append(Name, token, options);
    }

    public static void Clear(HttpResponse response) => response.Cookies.Delete(Name, CreateOptions());

    private static CookieOptions CreateOptions() => new()
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
        Path = "/api/identity"
    };
}
