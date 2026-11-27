using Carter;
using IdentityService.Features.Identity.Login.DTOs;
using IdentityService.Features.Identity.RefreshTokens.Commands;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.RefreshTokens.Controllers;

public class RefreshTokenController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity/refresh", async (
            HttpRequest request, HttpResponse response, ISender sender, CancellationToken cancellationToken) =>
        {
            response.Headers.CacheControl = "no-store";
            var result = await sender.Send(new RefreshTokenCommand(RefreshTokenCookie.Read(request)), cancellationToken);
            if (result is null)
            {
                return Results.Json(RequestResponse<object>.Fail("The refresh token is invalid or has expired.", 401),
                    statusCode: StatusCodes.Status401Unauthorized);
            }

            RefreshTokenCookie.Write(response, result.RefreshToken, result.RefreshExpiresAt);
            return Results.Ok(RequestResponse<LoginResponseDto>.Ok(new LoginResponseDto { AccessToken = result.AccessToken }));
        })
        .AllowAnonymous()
        .WithName("RefreshToken")
        .WithTags("Identity")
        .Produces<RequestResponse<LoginResponseDto>>(StatusCodes.Status200OK)
        .Produces<RequestResponse<object>>(StatusCodes.Status401Unauthorized)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests);
    }
}
