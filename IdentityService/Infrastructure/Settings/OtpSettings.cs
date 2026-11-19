namespace IdentityService.Infrastructure.Settings;

public class OtpSettings
{
    public const string SectionName = "OtpSettings";
    public int Length { get; set; } = 6;
    public int ExpirationMinutes { get; set; } = 10;
    public int MaxFailedAttempts { get; set; } = 5;
    public string ProtectionKey { get; set; } = string.Empty;

    public bool HasValidProtectionKey()
    {
        if (string.IsNullOrWhiteSpace(ProtectionKey))
            return false;

        try
        {
            return Convert.FromBase64String(ProtectionKey).Length >= 32;
        }
        catch (FormatException)
        {
            return false;
        }
    }
}
