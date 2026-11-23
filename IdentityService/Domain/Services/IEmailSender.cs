namespace IdentityService.Domain.Services;

public interface IEmailSender
{
    Task<EmailDeliveryStatus> SendAsync(
        string email, string subject, string body, CancellationToken cancellationToken = default);
}
