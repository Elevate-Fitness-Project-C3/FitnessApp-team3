using System.Globalization;
using System.Text.Encodings.Web;

namespace IdentityService.Features.Shared.Emails;

internal static class OtpEmailTemplate
{
    public static (string PlainText, string Html) Render(
        string heading, string instructions, string code, DateTimeOffset expiresAt, string fallback)
    {
        var expiry = expiresAt.UtcDateTime.ToString("dd MMM yyyy 'at' HH:mm:ss 'UTC'", CultureInfo.InvariantCulture);
        var plainText = $"""
            FitnessApp

            {heading}
            {instructions}

            Your one-time code
            {code}
            Expires {expiry}.

            {fallback}
            """;

        var encoder = HtmlEncoder.Default;
        var html = $$"""
            <!doctype html>
            <html lang="en">
            <head>
                <meta charset="utf-8">
                <meta name="viewport" content="width=device-width, initial-scale=1">
                <title>{{encoder.Encode(heading)}} | FitnessApp</title>
            </head>
            <body style="margin:0; padding:0; background-color:#eef4f0;">
                <table role="presentation" width="100%" cellspacing="0" cellpadding="0" bgcolor="#eef4f0">
                    <tr>
                        <td align="center" style="padding:32px 12px;">
                            <table role="presentation" width="520" cellspacing="0" cellpadding="0"
                                   style="width:100%; max-width:520px; font-family:Arial,sans-serif;">
                                <tr>
                                    <td bgcolor="#17352b" style="padding:20px 28px; border-radius:14px 14px 0 0;
                                        color:#e2fc94; font-size:20px; line-height:26px; font-weight:bold;">
                                        FitnessApp
                                    </td>
                                </tr>
                                <tr>
                                    <td bgcolor="#ffffff" style="padding:28px; border-radius:0 0 14px 14px; color:#142820;">
                                        <h1 style="margin:0 0 12px; font-size:26px; line-height:32px;">{{encoder.Encode(heading)}}</h1>
                                        <p style="margin:0 0 24px; font-size:16px; line-height:24px;">{{encoder.Encode(instructions)}}</p>
                                        <table role="presentation" width="100%" cellspacing="0" cellpadding="0">
                                            <tr>
                                                <td align="center" bgcolor="#eaf4df" style="padding:16px 12px; border-radius:10px;
                                                    color:#17352b;">
                                                    <p style="margin:0 0 6px; font-size:14px; line-height:20px;">Your one-time code</p>
                                                    <p dir="ltr" style="margin:0; font-family:'Courier New',monospace;
                                                        font-size:32px; line-height:40px; font-weight:bold; letter-spacing:0.12em;">{{encoder.Encode(code)}}</p>
                                                </td>
                                            </tr>
                                        </table>
                                        <p style="margin:12px 0 0; font-size:14px; line-height:20px; color:#41564a;">
                                            Expires {{encoder.Encode(expiry)}}.
                                        </p>
                                        <p style="margin:24px 0 0; padding-top:16px; border-top:1px solid #dce6df;
                                            font-size:14px; line-height:20px; color:#41564a;">{{encoder.Encode(fallback)}}</p>
                                    </td>
                                </tr>
                            </table>
                        </td>
                    </tr>
                </table>
            </body>
            </html>
            """;

        return (plainText, html);
    }
}
