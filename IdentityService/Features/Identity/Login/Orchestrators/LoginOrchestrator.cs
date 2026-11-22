using IdentityService.Common.Middleware;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Login.Commands;
using IdentityService.Features.Identity.Login.DTOs;
using IdentityService.Features.Identity.Login.Queries;
using MediatR;

namespace IdentityService.Features.Identity.Login.Orchestrators;

public class LoginOrchestrator(
    ISender sender,
    IUnitOfWork unitOfWork,
    TimeProvider timeProvider) : IRequestHandler<LoginCommand, LoginResult>
{
    public async Task<LoginResult> Handle(LoginCommand command, CancellationToken cancellationToken)
    {
        LoginResult? result = null;
        await unitOfWork.ExecuteAsync(async () =>
        {
            while (true)
            {
                var user = await sender.Send(new GetLoginAccountQuery(command.Email), cancellationToken);
                var accepted = await sender.Send(new VerifyLoginPasswordQuery(user, command.Password), cancellationToken);
                var now = timeProvider.GetUtcNow();
                if (user is null || !user.IsEnabled || !user.IsEmailVerified || user.LockoutEnd > now)
                    return;

                var updated = await sender.Send(new UpdateLoginAttemptsCommand(user, accepted, now), cancellationToken);
                if (!updated)
                    continue;
                if (!accepted)
                    return;

                var refresh = await sender.Send(new CreateRefreshTokenCommand(user.Id, now), cancellationToken);
                var accessToken = await sender.Send(new CreateAccessTokenCommand(user.Id, user.Role, now), cancellationToken);
                result = new LoginResult
                {
                    Response = new LoginResponseDto { AccessToken = accessToken },
                    RefreshToken = refresh.Token,
                    RefreshExpiresAt = refresh.ExpiresAt
                };
                return;
            }
        }, cancellationToken);

        // Return a failure only after committing the attempt counter.
        return result ?? throw new InvalidLoginCredentialsException();
    }
}
