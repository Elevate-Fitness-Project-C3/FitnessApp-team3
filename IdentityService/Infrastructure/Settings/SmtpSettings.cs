using System.Net.Mail;

namespace IdentityService.Infrastructure.Settings;

public class SmtpSettings
{
    public const string SectionName = "SmtpSettings";
    public string Host { get; set; } = string.Empty;
    public int Port { get; set; } = 587;
    public string Security { get; set; } = "StartTls";
    public string SenderName { get; set; } = "FitnessApp";
    public string SenderEmail { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public int TimeoutSeconds { get; set; } = 15;

    public bool IsValid()
        => !string.IsNullOrWhiteSpace(Host)
            && Port is > 0 and <= 65535
            && Security is "StartTls" or "SslOnConnect"
            && MailAddress.TryCreate(SenderEmail, out var address)
            && address.Address == SenderEmail
            && !SenderEmail.Contains("your-new-account", StringComparison.OrdinalIgnoreCase)
            && !string.IsNullOrWhiteSpace(Username)
            && !string.IsNullOrWhiteSpace(Password)
            && !Password.Contains('<')
            && TimeoutSeconds is >= 1 and <= 60;
}
