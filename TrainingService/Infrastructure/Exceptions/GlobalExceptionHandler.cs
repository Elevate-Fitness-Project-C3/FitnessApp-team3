using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

namespace TrainingService.Infrastructure.Exceptions;

public class GlobalExceptionHandler : IExceptionHandler {

    private readonly ILogger<GlobalExceptionHandler> logger;

    public GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) {

        this.logger = logger;
    }

    public async ValueTask<bool> TryHandleAsync(HttpContext httpContext,Exception exception,CancellationToken cancellationToken) {

        switch (exception) {
            case ValidationException validationException:

            var errors = validationException.Errors
                .GroupBy(e => e.PropertyName.Split('.').Last())
                .ToDictionary(
                    g => char.ToLowerInvariant(g.Key[0])
                         + g.Key.Substring(1),
                    g => g.Select(e => e.ErrorMessage)
                          .Distinct()
                          .ToArray());

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await httpContext.Response.WriteAsJsonAsync(
                new {
                    Success = false,
                    StatusCode = 400,
                    Message = "Validation failed.",
                    Errors = errors
                },
                cancellationToken);

            return true;

            case KeyNotFoundException:

            httpContext.Response.StatusCode =
                StatusCodes.Status404NotFound;

            await httpContext.Response.WriteAsJsonAsync(
                new {
                    Success = false,
                    StatusCode = 404,
                    Message = "Requested resource was not found."
                },
                cancellationToken);

            return true;

            default:

            logger.LogError(
                exception,
                "An unhandled exception occurred.");

            httpContext.Response.StatusCode =
                StatusCodes.Status500InternalServerError;

            await httpContext.Response.WriteAsJsonAsync(
                new {
                    Success = false,
                    StatusCode = 500,
                    Message = "An unexpected error occurred."
                },
                cancellationToken);

            return true;
        }
    }
}