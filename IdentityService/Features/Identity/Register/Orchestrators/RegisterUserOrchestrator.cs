using FluentValidation;
using IdentityService.Common.Identity;
using IdentityService.Common.Middleware;
using IdentityService.Domain.Repositories;
using IdentityService.Domain.Services;
using IdentityService.Features.Identity.Register.Commands;
using IdentityService.Features.Identity.Register.DTOs;
using IdentityService.Features.Identity.Register.Queries;
using IdentityService.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using MediatR;

namespace IdentityService.Features.Identity.Register.Orchestrators;

public class RegisterUserOrchestrator(
    IValidator<RegisterUserCommand> validator,
    ISender sender,
    IUnitOfWork unitOfWork) : IRequestHandler<RegisterUserCommand, RegisterResponseDto>
{
    public const string DuplicateEmailMessage =
        "An account already uses this email. Sign in if verified, or request another verification code.";

    public async Task<RegisterResponseDto> Handle(
        RegisterUserCommand command, CancellationToken cancellationToken)
    {
        await validator.ValidateAndThrowAsync(command, cancellationToken);

        var normalizedEmail = EmailNormalizer.Normalize(command.Email);
        if (await sender.Send(new EmailExistsQuery(normalizedEmail), cancellationToken))
            throw new ConflictException(DuplicateEmailMessage);

        GeneratedEmailVerificationOtp verification = null!;
        try
        {
            await unitOfWork.ExecuteAsync(async () =>
            {
                var user = await sender.Send(
                    new CreateUserCommand(command.Name, command.Email, command.Password), cancellationToken);
                verification = await sender.Send(
                    new CreateEmailVerificationOtpCommand(user), cancellationToken);
            }, cancellationToken);
        }
        catch (DbUpdateException exception) when (SqlServerExceptionHelper.IsNormalizedEmailConflict(exception))
        {
            throw new ConflictException(DuplicateEmailMessage);
        }

        var user = verification.Challenge.User;
        // SMTP is external: send only after the account and challenge have committed.
        var delivery = await sender.Send(
            new SendVerificationEmailCommand(user.Email, verification.Code, verification.Challenge.ExpiresAt),
            cancellationToken);
        if (delivery != EmailDeliveryStatus.Accepted)
            throw new EmailDeliveryException(delivery);

        return new RegisterResponseDto
        {
            UserId = user.Id,
            Email = user.Email,
            Role = user.Role,
            IsEmailVerified = user.IsEmailVerified
        };
    }
}
