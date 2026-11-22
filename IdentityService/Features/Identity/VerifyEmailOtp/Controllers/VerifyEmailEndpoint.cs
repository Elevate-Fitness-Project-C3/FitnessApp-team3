using Carter;
using FluentValidation;
using IdentityService.Features.Identity.VerifyEmailOtp.Orchestrators;
using IdentityService.Features.Identity.VerifyEmailOtp.ViewModels;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Controllers
{
    public class VerifyEmailEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/identity/resend-otp/verify", async (VerifyEmailOtpVM request,
                IValidator<VerifyEmailOtpVM> validator, IMediator sender, CancellationToken cancellationToken) =>
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);
                var result = await sender.Send(new VerifyOtpOrchestrator(request.Email, request.Otp), cancellationToken);
                if (!result)
                    return Results.BadRequest(RequestResponse<bool>.Fail("Failed to verify OTP."));

                return Results.Json(RequestResponse<bool>.Created(true, "OTP verified successfully."),
                    statusCode: StatusCodes.Status201Created);
            }).AllowAnonymous()
            .RequireRateLimiting("registration")
        .WithName("VerifyOtp")
        .WithTags("Identity")
        .Produces<RequestResponse<bool>>(StatusCodes.Status201Created)
        .Produces<RequestResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests);
        }
    }
}
