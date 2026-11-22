using FluentValidation;
using FitnessProfileService.Common.Middleware;
using FitnessProfileService.Features.Shared;
//using FitnessProfileService.Features.Shared;


namespace FitnessProfileService.Common.Middleware;

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
                UnauthorizedAccessException => (StatusCodes.Status401Unauthorized,
                    "You are not authorized to access this resource."),
                KeyNotFoundException => (StatusCodes.Status404NotFound,
                    "The requested resource was not found."),
                BadRequestException => (StatusCodes.Status400BadRequest, exception.Message),
                NotFoundException => (StatusCodes.Status404NotFound, exception.Message),
                ConflictException => (StatusCodes.Status409Conflict, exception.Message),
                GoneException => (StatusCodes.Status410Gone, exception.Message),
                ValidationException => (StatusCodes.Status400BadRequest, "Validation failed."),
                _ => (StatusCodes.Status500InternalServerError, "An unexpected error occurred.")
            };

            if (statusCode >= 500)
                logger.LogError(exception, "An unhandled exception occurred.");
            else
                logger.LogWarning(exception, "Request failed with status code {StatusCode}.", statusCode);

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
