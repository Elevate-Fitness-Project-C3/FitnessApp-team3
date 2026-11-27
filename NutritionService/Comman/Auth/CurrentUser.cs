using System.Security.AccessControl;
using System.Security.Claims;

namespace NutritionService.Comman.Auth
{
    public static class CurrentUser
    {
        public static bool TryGetUserId(ClaimsPrincipal user , out Guid userId)
        {
            var raw = user.FindFirstValue("sub") ?? user.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.TryParse(raw, out userId);
        }
        public static Task<IResult> WithUserId(this ClaimsPrincipal user , Func<Guid,Task<IResult>> action)
        {
            if(!TryGetUserId(user,out var userId))
                return Task.FromResult(Microsoft.AspNetCore.Http.Results.Unauthorized());
            return action(userId);
        }
    }
}
