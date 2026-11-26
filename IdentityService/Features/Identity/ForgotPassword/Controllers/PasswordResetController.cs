using System.Net.Http.Headers;
using Carter;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.DTOs;
using IdentityService.Features.Shared;
using MediatR;
using Microsoft.OpenApi;

namespace IdentityService.Features.Identity.ForgotPassword.Controllers;

public class PasswordResetController : ICarterModule
{
    private const string InvalidResetTokenMessage = "The password reset token is invalid or expired.";
    private const string PasswordResetBearerScheme = "PasswordResetBearer";

    public void AddRoutes(IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/identity/password-reset").AllowAnonymous().WithTags("Identity");

        group.MapPost("/request", async (RequestPasswordResetDto request, ISender sender, CancellationToken cancellationToken) =>
        {
            var queued = await sender.Send(new RequestPasswordResetCommand(request.Email), cancellationToken);
            if (!queued)
                return Results.Json(RequestResponse.Fail("Password reset is temporarily unavailable. Try again later.", 503),
                    statusCode: StatusCodes.Status503ServiceUnavailable);

            return Results.Json(RequestResponse.Ok("If an account matches that email, a password reset code will be sent.", 202),
                statusCode: StatusCodes.Status202Accepted);
        })
        .RequireRateLimiting("password-reset-request")
        .WithName("RequestPasswordReset")
        .Produces<RequestResponse>(StatusCodes.Status202Accepted)
        .Produces<RequestResponse>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse>(StatusCodes.Status429TooManyRequests)
        .Produces<RequestResponse>(StatusCodes.Status503ServiceUnavailable);

        group.MapPost("/verify", async (VerifyPasswordResetDto request, ISender sender, HttpResponse response, CancellationToken cancellationToken) =>
        {
            var token = await sender.Send(new VerifyPasswordResetCommand(request.Email, request.Otp), cancellationToken);
            if (token is null)
                return Results.BadRequest(RequestResponse.Fail("The password reset code is invalid or expired."));

            response.Headers.CacheControl = "no-store";
            return Results.Ok(RequestResponse<PasswordResetTokenDto>.Ok(token));
        })
        .RequireRateLimiting("password-reset-verify")
        .WithName("VerifyPasswordReset")
        .Produces<RequestResponse<PasswordResetTokenDto>>(StatusCodes.Status200OK)
        .Produces<RequestResponse>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse>(StatusCodes.Status429TooManyRequests);

        group.MapPost("/complete", async (CompletePasswordResetDto request, HttpRequest httpRequest, ISender sender,
            CancellationToken cancellationToken) =>
        {
            var authorization = httpRequest.Headers.Authorization;
            if (authorization.Count != 1 || !AuthenticationHeaderValue.TryParse(authorization[0], out var header)
                || !string.Equals(header.Scheme, "Bearer", StringComparison.OrdinalIgnoreCase)
                || string.IsNullOrEmpty(header.Parameter))
                return Results.BadRequest(RequestResponse.Fail(InvalidResetTokenMessage));

            var completed = await sender.Send(
                new CompletePasswordResetCommand(header.Parameter, request.NewPassword), cancellationToken);
            return completed
                ? Results.NoContent()
                : Results.BadRequest(RequestResponse.Fail(InvalidResetTokenMessage));
        })
        .RequireRateLimiting("password-reset-complete")
        .WithName("CompletePasswordReset")
        .AddOpenApiOperationTransformer((operation, context, _) =>
        {
            if (context.Document is not { } document)
                return Task.CompletedTask;

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes[PasswordResetBearerScheme] = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                Description = "Use the reset token returned by password reset verification."
            };
            operation.Security =
            [
                new OpenApiSecurityRequirement
                {
                    [new OpenApiSecuritySchemeReference(PasswordResetBearerScheme, document)] = []
                }
            ];
            return Task.CompletedTask;
        })
        .Produces(StatusCodes.Status204NoContent)
        .Produces<RequestResponse>(StatusCodes.Status400BadRequest)
        .Produces<RequestResponse>(StatusCodes.Status429TooManyRequests);
    }
}
