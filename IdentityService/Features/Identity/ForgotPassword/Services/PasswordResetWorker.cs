using IdentityService.Features.Identity.ForgotPassword.Commands;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Services;

public class PasswordResetWorker(
    PasswordResetQueue queue, IServiceScopeFactory scopeFactory, ILogger<PasswordResetWorker> logger)
    : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await foreach (var email in queue.ReadAllAsync(stoppingToken))
        {
            try
            {
                await using var scope = scopeFactory.CreateAsyncScope();
                var sender = scope.ServiceProvider.GetRequiredService<ISender>();
                await sender.Send(new SendPasswordResetCommand(email), stoppingToken);
            }
            catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
            {
                return;
            }
            catch (Exception exception)
            {
                logger.LogError("Password reset request processing failed with {ExceptionType}.", exception.GetType().Name);
            }
        }
    }
}
