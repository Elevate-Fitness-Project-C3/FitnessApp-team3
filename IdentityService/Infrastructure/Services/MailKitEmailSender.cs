using System.Net.Sockets;
using IdentityService.Domain.Services;
using IdentityService.Infrastructure.Settings;
using MailKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace IdentityService.Infrastructure.Services;

public class MailKitEmailSender(ISmtpClient client, IOptions<SmtpSettings> options) : IEmailSender
{
    public async Task<EmailDeliveryStatus> SendAsync(
        string email, string subject, string body, CancellationToken cancellationToken = default)
    {
        var settings = options.Value;
        using var timeout = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
        timeout.CancelAfter(TimeSpan.FromSeconds(settings.TimeoutSeconds));
        var deliveryStatus = EmailDeliveryStatus.Unavailable;

        try
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(settings.SenderName, settings.SenderEmail));
            message.To.Add(new MailboxAddress(string.Empty, email));
            message.Subject = subject;
            message.Body = new TextPart("plain") { Text = body };

            var security = settings.Security == "SslOnConnect"
                ? SecureSocketOptions.SslOnConnect
                : SecureSocketOptions.StartTls;

            await client.ConnectAsync(settings.Host, settings.Port, security, timeout.Token);
            await client.AuthenticateAsync(settings.Username, settings.Password, timeout.Token);
            deliveryStatus = EmailDeliveryStatus.Unknown;
            await client.SendAsync(message, timeout.Token);
            deliveryStatus = EmailDeliveryStatus.Accepted;
            return deliveryStatus;
        }
        catch (OperationCanceledException) when (cancellationToken.IsCancellationRequested)
        {
            throw;
        }
        catch (SmtpCommandException)
        {
            return EmailDeliveryStatus.Rejected;
        }
        catch (Exception exception) when (exception is OperationCanceledException
            or TimeoutException or IOException or SocketException or SmtpProtocolException
            or MailKit.Security.AuthenticationException or ServiceNotAuthenticatedException
            or ServiceNotConnectedException or SslHandshakeException or NotSupportedException)
        {
            return deliveryStatus;
        }
        finally
        {
            if (client.IsConnected)
            {
                try
                {
                    // Cleanup cannot change a confirmed SMTP submission into a delivery failure.
                    using var cleanup = new CancellationTokenSource(TimeSpan.FromSeconds(2));
                    await client.DisconnectAsync(deliveryStatus == EmailDeliveryStatus.Accepted, cleanup.Token);
                }
                catch (Exception exception) when (exception is IOException or SocketException
                    or OperationCanceledException or TimeoutException or SmtpProtocolException or SmtpCommandException)
                {
                }
            }
        }
    }
}
