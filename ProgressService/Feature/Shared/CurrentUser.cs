using System.Security.Claims;

namespace ProgressService.Feature.Shared;

public static class CurrentUser
{
    public static bool TryGetId(ClaimsPrincipal user, out Guid userId)
    {
        return Guid.TryParse(
            user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier),
            out userId);
    }
}
