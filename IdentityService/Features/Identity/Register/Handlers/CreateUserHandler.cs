using IdentityService.Common.Constants;
using IdentityService.Common.Identity;
using IdentityService.Domain.Entities.Identity;
using IdentityService.Domain.Repositories;
using IdentityService.Features.Identity.Register.Commands;
using Microsoft.AspNetCore.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Register.Handlers;

public class CreateUserHandler(
    IUnitOfWork unitOfWork,
    IPasswordHasher<ApplicationUser> passwordHasher,
    TimeProvider timeProvider) : IRequestHandler<CreateUserCommand, ApplicationUser>
{
    public async Task<ApplicationUser> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        ApplicationUser user = null!;
        await unitOfWork.ExecuteAsync(() =>
        {
            user = new ApplicationUser
            {
                Name = command.Name.Trim(),
                Email = command.Email.Trim(),
                NormalizedEmail = EmailNormalizer.Normalize(command.Email),
                PasswordHash = string.Empty,
                Role = Roles.User,
                IsEmailVerified = false,
                CreatedAt = timeProvider.GetUtcNow()
            };
            user.PasswordHash = passwordHasher.HashPassword(user, command.Password);
            unitOfWork.GetRepository<ApplicationUser>().Add(user);
            return Task.CompletedTask;
        }, cancellationToken);
        return user;
    }
}
