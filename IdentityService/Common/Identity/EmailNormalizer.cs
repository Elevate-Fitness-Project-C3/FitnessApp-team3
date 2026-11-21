namespace IdentityService.Common.Identity;

public static class EmailNormalizer
{
    public static string Normalize(string email) => email.Trim().ToUpperInvariant();
}
