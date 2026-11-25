using MassTransit;

namespace IdentityService.Features.Identity.Register.Events;

[EntityName("user-created")]
public record UserCreated(int UserId);
