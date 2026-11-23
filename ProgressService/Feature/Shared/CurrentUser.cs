using System.Security.Claims;

namespace ProgressService.Feature.Shared;

public static class CurrentUser
{
    public static bool TryGetId(ClaimsPrincipal user, out Guid userId)
    {
        return Guid.TryParse(
            user.FindFirst("sub")?.Value ?? user.FindFirst(ClaimTypes.NameIdentifier)?.Value,
            out userId);
    }
}
