using Carter;
using IdentityService.Features.Identity.Logout.Commands;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.Logout.Controllers;

public class LogoutController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity/logout", async (
            HttpRequest request, HttpResponse response, ISender sender, CancellationToken cancellationToken) =>
        {
            response.Headers.CacheControl = "no-store";
            await sender.Send(new LogoutCommand(RefreshTokenCookie.Read(request)), cancellationToken);
            RefreshTokenCookie.Clear(response);
            return Results.NoContent();
        })
        .AllowAnonymous()
        .WithName("Logout")
        .WithTags("Identity")
        .Produces(StatusCodes.Status204NoContent)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests);
    }
}
