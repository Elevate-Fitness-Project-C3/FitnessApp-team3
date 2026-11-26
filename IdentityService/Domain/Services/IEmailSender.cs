namespace IdentityService.Domain.Services;

public interface IEmailSender
{
    Task<EmailDeliveryStatus> SendAsync(
        string email, string subject, string body, CancellationToken cancellationToken = default);

    Task<EmailDeliveryStatus> SendAsync(
        string email, string subject, string body, string? htmlBody, CancellationToken cancellationToken)
        => SendAsync(email, subject, body, cancellationToken);
}
