using Carter;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.DTOs;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.Register.Controllers;

public class RegisterController : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/api/identity/register", async (
            RegisterRequestDto request, ISender sender, CancellationToken cancellationToken) =>
        {
            var response = await sender.Send(
                new RegisterUserCommand(request.Name, request.Email, request.Password), cancellationToken);
            return Results.Json(
                RequestResponse<RegisterResponseDto>.Created(response,
                    "Account created. Check your email for the verification code."),
                statusCode: StatusCodes.Status201Created);
        })
        .AllowAnonymous()
        .RequireRateLimiting("registration")
        .WithName("RegisterUser")
        .WithTags("Identity")
        .Produces<RequestResponse<RegisterResponseDto>>(StatusCodes.Status201Created)
        .Produces<RequestResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse<object>>(StatusCodes.Status409Conflict)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests)
        .Produces<RequestResponse<object>>(StatusCodes.Status503ServiceUnavailable);
    }
}
