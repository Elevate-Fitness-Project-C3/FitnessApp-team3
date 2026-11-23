using FluentValidation;
using IdentityService.Features.Shared;

namespace IdentityService.Common.Middleware;

public class GlobalExceptionMiddleware(
    RequestDelegate next,
    ILogger<GlobalExceptionMiddleware> logger)
{
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await next(context);
        }
        catch (OperationCanceledException) when (context.RequestAborted.IsCancellationRequested)
        {
            throw;
        }
        catch (Exception exception) when (!context.Response.HasStarted)
        {
            var (statusCode, message) = exception switch
            {
                InvalidLoginCredentialsException => (StatusCodes.Status401Unauthorized, exception.Message),
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                    "You are not authorized to access this resource."),
                KeyNotFoundException => (StatusCodes.Status404NotFound,
                    "The requested resource was not found."),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                ConflictException => (StatusCodes.Status409Conflict, exception.Message),
                GoneException => (StatusCodes.Status410Gone, exception.Message),
                ValidationException => (StatusCodes.Status400BadRequest, "Validation failed."),
                BadHttpRequestException badRequest => (badRequest.StatusCode, "Invalid request."),
                EmailDeliveryException => (StatusCodes.Status503ServiceUnavailable, exception.Message),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            if (statusCode >= 500)
                logger.LogError("Request failed with {ExceptionType}, status {StatusCode}, trace {TraceId}.",
                    exception.GetType().Name, statusCode, context.TraceIdentifier);
            else
                logger.LogWarning("Request failed with {ExceptionType}, status {StatusCode}, trace {TraceId}.",
                    exception.GetType().Name, statusCode, context.TraceIdentifier);

            var errors = exception is ValidationException validation
                ? validation.Errors.GroupBy(error => error.PropertyName)
                    .ToDictionary(group => group.Key,
                        group => group.Select(error => error.ErrorMessage).Distinct().ToArray())
                : null;

            context.Response.Clear();
            context.Response.StatusCode = statusCode;
            await context.Response.WriteAsJsonAsync(
                RequestResponse<object>.Fail(message, statusCode, errors),
                cancellationToken: context.RequestAborted);
        }
    }
}
