# Access token verification and key setup

IdentityService signs access tokens with an RSA private key. Fitness Profile,
Nutrition, Payment, Progress, and Training each verify those tokens locally
with the matching public key.

## How a token reaches Training Service

1. Register with `POST /api/identity/register`. Use an email address you control
   and submit the code with `POST /api/identity/resend-otp/verify` and a JSON body
   containing `email` and `otp`. To request a new code, send `userEmail` to
   `POST /api/identity/resend-otp`.
2. Send `email` and `password` to `POST /api/identity/login`. A verified,
   enabled account with valid credentials receives `data.accessToken`.
   IdentityService signs the JWT using `RS256` and includes the signing key ID
   (`kid`), user ID (`sub`), role, issuer, audience, and expiry. The access
   token lasts 15 minutes by default.
3. Send `Authorization: Bearer <accessToken>` to Training Service.
   `FitnessApp.Authentication` is a class library that each API builds into its
   application. Its `AddAccessTokenAuthentication` method configures ASP.NET Core
   JWT bearer authentication with a local public PEM. The middleware checks the
   RS256 signature, key ID, issuer, audience, and expiry before a protected
   endpoint runs. It uses `sub` as the user ID and `role` for role checks. Token
   validation requires no request to IdentityService.

IdentityService also sends an opaque `refresh_token` cookie scoped to
`/api/identity`. The cookie is `Secure` and `HttpOnly`, and the database stores
its hash. Send the access JWT in API requests. Refresh rotation is part of
SCRUM-15. For now, sign in again when the access token expires.

[Shared validator](../FitnessApp.Authentication/AccessTokenAuthentication.cs) ·
[Training API example](../TrainingService/Program.cs)

## Configure the keys for Docker Compose

Run commands from the repository root. If you are setting up a fresh checkout,
copy `.env.example` to `.env` and fill in your local SQL Server, RabbitMQ, SMTP,
and OTP settings. Generate a separate key pair for each independent development
environment. Reuse the existing pair if your environment already has one.
Keep the private PEM in the Git-ignored `.env` or IdentityService's runtime
secret store. Share the public PEM with the consuming APIs.

| Setting in `.env` | Purpose |
| --- | --- |
| `JWT_PRIVATE_KEY_PEM` | RSA private key used only by IdentityService |
| `JWT_PUBLIC_KEY_PEM` | Matching RSA public key used by the APIs |
| `JWT_ISSUER` | `FitnessApp.Identity` by default |
| `JWT_AUDIENCE` | `FitnessApp.Api` by default |
| `JWT_KEY_ID` | `fitness-1` by default |

With OpenSSL on your path, this PowerShell block generates a 3072-bit pair and
fills the two empty entries from the template. Run it once for a fresh
environment. It stops if either key entry already has a value.

```powershell
$envFile = (Resolve-Path .env).Path
$settings = [IO.File]::ReadAllText($envFile)
$privateEntry = [regex]'(?m)^JWT_PRIVATE_KEY_PEM=[ \t]*(?=\r?$)'
$publicEntry = [regex]'(?m)^JWT_PUBLIC_KEY_PEM=[ \t]*(?=\r?$)'
if ($privateEntry.Matches($settings).Count -ne 1 -or $publicEntry.Matches($settings).Count -ne 1) {
    throw 'Expected one empty private PEM entry and one empty public PEM entry in .env.'
}

$privatePem = @(openssl genpkey -algorithm RSA -pkeyopt rsa_keygen_bits:3072 -quiet)
if ($LASTEXITCODE -ne 0 -or !$privatePem) { throw 'RSA key generation failed.' }
$publicPem = @($privatePem | openssl pkey -pubout)
if ($LASTEXITCODE -ne 0 -or !$publicPem) { throw 'Public key export failed.' }
$settings = $privateEntry.Replace($settings, "JWT_PRIVATE_KEY_PEM='$($privatePem -join "`n")'")
$settings = $publicEntry.Replace($settings, "JWT_PUBLIC_KEY_PEM='$($publicPem -join "`n")'")
[IO.File]::WriteAllText($envFile, $settings)

docker compose --env-file .env config --quiet
if ($LASTEXITCODE -ne 0) { throw 'Docker Compose could not load .env.' }
docker compose up --build -d
```

To configure an existing pair manually, put each complete PEM in the matching
`.env` entry with single quotes around the multiline value. Keep the opening
and closing PEM lines. Compose sends the private PEM only to IdentityService.
Use the same issuer, audience, and key ID across the stack. For SMTP and OTP
configuration, see [IdentityService setup](../IdentityService/README.md#gmail-setup).

Apply the IdentityService EF migrations to `FitnessIdentity` before testing
login. Compose creates the database and EF creates or upgrades its schema.
When running EF on the host against Docker SQL Server, set
`ConnectionStrings__DefaultConnection` to the connection for
`localhost,1433` and `Database=FitnessIdentity`. An old schema can produce a
`500` with missing login columns such as `RowVersion`. [Migration command and
connection setup](../IdentityService/README.md#configure-persistence)

## Run directly with .NET

Use the .NET 10 SDK and the services' `https` launch profiles. Set
`JwtSettings:PrivateKeyPem` in **IdentityService user secrets** and
`JwtSettings:PublicKeyPem` in **each consuming API's user secrets**. The export
command reads IdentityService's configured signer, so it can export the matching
public key from a pair that already exists. [Generate, export, and install a PEM
in user secrets](../IdentityService/README.md#configure-signing-and-export-the-public-pem)

## Test login and token verification

| How you run the stack | IdentityService login | Training Service test |
| --- | --- | --- |
| .NET HTTPS profiles | `https://localhost:7220/api/identity/login` | `https://localhost:7091/weatherforecast` |
| Docker Compose on localhost | `http://localhost:5002/api/identity/login` | `http://localhost:5296/weatherforecast` |

For a local Compose smoke test, open `http://localhost:5002/swagger`, sign in
with your verified account, and copy `data.accessToken`. For the .NET HTTPS
profile, Swagger runs at `https://localhost:7220/swagger`. Login is anonymous.

```http
GET https://localhost:7091/weatherforecast
Authorization: Bearer <accessToken>
```

Training returns `200` with a valid token and `401` with a missing, invalid, or
expired token. If login succeeds but Training rejects the token, check the
bearer header, token expiry, and whether the loaded public PEM, issuer,
audience, and key ID match the signer. For browser testing with the secure
refresh cookie, use the .NET HTTPS profile or HTTPS ingress for Compose.

