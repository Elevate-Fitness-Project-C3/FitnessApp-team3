using Carter;
using FluentValidation;
using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Features.Identity.Login.DTOs;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.Login.Controllers;

public class LoginController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity/login", async (
            LoginRequestDto request, IValidator<LoginRequestDto> validator, ISender sender,
            HttpResponse response, CancellationToken cancellationToken) =>
        {
            await validator.ValidateAndThrowAsync(request, cancellationToken);
            var result = await sender.Send(new LoginCommand(request.Email, request.Password), cancellationToken);
            response.Headers.CacheControl = "no-store";
            RefreshTokenCookie.Write(response, result.RefreshToken, result.RefreshExpiresAt);
            return Results.Ok(RequestResponse<LoginResponseDto>.Ok(result.Response));
        })
        .AllowAnonymous()
        .RequireRateLimiting("login")
        .WithName("Login")
        .WithTags("Identity")
        .Produces<RequestResponse<LoginResponseDto>>(StatusCodes.Status200OK)
        .Produces<RequestResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse<object>>(StatusCodes.Status401Unauthorized)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests);
    }
}
