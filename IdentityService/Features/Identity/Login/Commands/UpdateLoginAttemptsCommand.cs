using IdentityService.Domain.Entities.Identity;
using MediatR;

namespace IdentityService.Features.Identity.Login.Commands;

public record UpdateLoginAttemptsCommand(ApplicationUser User, bool PasswordAccepted, DateTimeOffset Now)
    : IRequest<bool>;
