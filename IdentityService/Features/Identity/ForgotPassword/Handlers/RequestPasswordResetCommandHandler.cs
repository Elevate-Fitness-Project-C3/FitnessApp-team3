using FluentValidation;
using IdentityService.Features.Identity.ForgotPassword.Commands;
using IdentityService.Features.Identity.ForgotPassword.Services;
using MediatR;

namespace IdentityService.Features.Identity.ForgotPassword.Handlers;

public class RequestPasswordResetCommandHandler(
    IValidator<RequestPasswordResetCommand> validator,
    PasswordResetQueue queue) : IRequestHandler<RequestPasswordResetCommand, bool>
{
    public async Task<bool> Handle(RequestPasswordResetCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);
        return queue.TryEnqueue(command.Email.Trim());
    }
}
