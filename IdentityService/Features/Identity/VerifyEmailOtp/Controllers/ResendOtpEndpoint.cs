using Carter;
using FluentValidation;
using IdentityService.Features.Identity.VerifyEmailOtp.Orchestrators;
using IdentityService.Features.Identity.VerifyEmailOtp.ViewModels;
using IdentityService.Features.Shared;
using MediatR;

namespace IdentityService.Features.Identity.VerifyEmailOtp.Controllers
{
    public class ResendOtpEndpoint : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/identity/resend-otp", async (ResendEmailVerifycationOtpVM request,
                IValidator<ResendEmailVerifycationOtpVM> validator, IMediator sender, CancellationToken cancellationToken) =>
            {
                await validator.ValidateAndThrowAsync(request, cancellationToken);
                var result = await sender.Send(new ResendEmailVerifycationOtpOrchestrator(request.UserEmail), cancellationToken);
                if (!result)
                    return Results.BadRequest(RequestResponse<bool>.Fail("Failed to resend OTP."));

                return Results.Json(RequestResponse<bool>.Created(true, "OTP resent successfully. Check your email."),
                    statusCode: StatusCodes.Status201Created);
            }).AllowAnonymous()
        .RequireRateLimiting("registration")
        .WithName("ResendOtp")
        .WithTags("Identity")
        .Produces<RequestResponse<bool>>(StatusCodes.Status201Created)
        .Produces<RequestResponse<object>>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse<object>>(StatusCodes.Status429TooManyRequests)
        .Produces<RequestResponse<object>>(StatusCodes.Status503ServiceUnavailable);
        }
    }
}
