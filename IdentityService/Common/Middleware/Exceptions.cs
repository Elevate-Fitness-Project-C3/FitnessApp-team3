using IdentityService.Domain.Services;

namespace IdentityService.Common.Middleware;

public class NotFoundException(string message) : Exception(message);
public class ConflictException(string message) : Exception(message);
public class GoneException(string message) : Exception(message);

public class EmailDeliveryException(EmailDeliveryStatus status) : Exception(
    "Your account was created, but verification email delivery could not be confirmed. "
    + "If the email arrives, its code remains valid until it expires. Otherwise, request another verification code.")
{
    public EmailDeliveryStatus Status { get; } = status;
}
