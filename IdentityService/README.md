# IdentityService

The identity feature layout follows [exam-system-team-5](https://github.com/Elevate-Exam-Projects-c3/exam-system-team-5/tree/e6af33a69e3725bdc1fde5b1aea4d0e0247d3562/Features/Identity/Register).
Repository placement and contracts follow [LMS Mini Version](https://github.com/Amr-shawky/LMS---Mini-Version/tree/1a63882d1743ecdc5ed005b4dcf14ce129a5836a/Infrastructure/Repositories).
MediatR dispatch and cached repository access follow
[SUENO](https://github.com/SEUNO-GRAD/SUENO-BE/tree/develop).

## Structure

- `Common/Middleware`: global exception middleware and shared exceptions.
- `Common/Results`: `Result` and `Result<T>`.
- `Domain/Common`: `BaseEntity` with an integer `Id`, matching the LMS contract.
- `Domain/Entities/Identity`: `ApplicationUser`, `OtpCode`, and `RefreshToken` entities.
- `Domain/Repositories`: `IGeneralRepository<T>` and `IUnitOfWork`.
- `Features/Identity`: ForgotPassword, Login, Logout, RefreshTokens, Register,
  VerifyEmailOtp, and shared ViewModels folders.
- `Features/Shared`: HTTP response envelope and shared DTO scaffold.
- `Infrastructure/Persistence`: `AppDbContext` and entity configurations.
- `Infrastructure/Repositories`: `GeneralRepository<T>` and `UnitOfWork`.
- `Infrastructure/DependencyInjection.cs`: scoped persistence registrations.
- `Migrations`: SQL Server schema migrations and the EF model snapshot.

`Features/Identity/Register` contains `Commands`, `Controllers`, `DTOs`,
`Events`, `Handlers`, `Orchestrators`, `Queries`, and `Validators`, matching
the reference. Other operations retain the DTO, event, and view model folders
present in that reference. Empty folders contain `.gitkeep` files.

Registration, email verification, resend, password login, refresh rotation, and logout are implemented.
Password reset is a separate feature.

## API startup

`Program.cs` registers Carter and maps minimal API routes from `ICarterModule`
implementations. Add feature endpoints in Carter modules; MVC controllers are
not registered.

Run the service with the HTTPS development profile:

```powershell
dotnet run --project IdentityService/IdentityService.csproj --launch-profile https
```

Swagger UI is available at `https://localhost:7220/swagger`, using the generated
OpenAPI document at `/openapi/v1.json`. Both are enabled only in Development.
The HTTP and HTTPS launch profiles open Swagger UI when launched from an IDE.
The document includes the anonymous registration operation.

The built-in global rate limiter allows 100 requests per minute per client IP,
with no queue. Registration additionally allows five requests per IP per fifteen
minutes. Excess requests return HTTP 429 with the response envelope and
`Retry-After` when available. Limits are held in memory per
service instance. Behind a reverse proxy, configure trusted forwarded headers
before rate limiting so clients are identified by their actual IP.

## Base file behavior

`GeneralRepository<T>` supports queries, CRUD, explicit saves, and `SaveInclude`
for selected scalar property updates. Partial updates support detached objects
and objects already tracked by the context. Write methods do not save automatically.

`UnitOfWork.ExecuteAsync(Func<Task>, CancellationToken)` uses a depth counter.
The outer call opens the transaction; nested command-handler calls share it.
Only the outer call saves and commits. Exceptions and cancellation roll back the
outer transaction and clear tracking. A caught nested failure still prevents the
outer call from committing. `SaveChangesAsync` can flush changes within that
boundary without committing it.

`AddSavePointAsync` flushes pending changes before creating a named savepoint.
`RollbackToSavePointAsync` rolls back to that point and clears tracking; reload
entities before continuing. Both methods require an active UOW transaction.

Inject `IUnitOfWork` into handlers and obtain repositories through
`unitOfWork.GetRepository<TEntity>()`, for example
`unitOfWork.GetRepository<ApplicationUser>()`. Each unit of work caches one
repository per entity type, and all its repositories share the scoped `AppDbContext`.
Repository access, saves, and transactions are rejected after a failed operation;
retry through a fresh service scope. `CommitMayHaveSucceeded` becomes true immediately
before commit is attempted, so a connection failure cannot be mistaken for a
confirmed rollback. Cleanup logs only controlled metadata and preserves the
original operation failure.
The unit of work owns its transaction; the service scope owns the context.

`GlobalExceptionMiddleware` is registered in `Program.cs`. It returns a
`RequestResponse<T>` JSON envelope for validation (400), unauthorized (401),
not found (404), conflict (409), gone (410), email-delivery (503), and unexpected
(500) errors. Malformed input receives its HTTP error status in the same envelope.
Logs contain exception types, status codes, and trace identifiers, not raw
exceptions or request objects. EF command/update logging is disabled in the
supplied configuration; keep sensitive-data and SMTP protocol logging disabled.
Unexpected errors return a generic message. Request cancellation
propagates, and responses that have started are not overwritten.

## Configure persistence

`Program.cs` registers the SQL Server EF Core provider through
`AddIdentityPersistence`, using `ConnectionStrings:DefaultConnection`. Docker
Compose supplies this setting for the `FitnessIdentity` database. Running locally
in Development defaults to Windows LocalDB; override the connection string with
user secrets or the `ConnectionStrings__DefaultConnection` environment variable
to use a different SQL Server instance. Production requires an explicit connection
string.

The initial migration creates the three tables from the identity diagram.
`AddUserRegistration` and `AddLoginProtection` extend them for registration and login.

| Table | Primary key | Other columns |
| --- | --- | --- |
| `ApplicationUser` | `UserId` | `Name`, `Email`, `NormalizedEmail`, `PasswordHash`, `Role`, `IsEmailVerified`, `CreatedAt`, `IsEnabled`, `FailedLoginAttempts`, `LockoutEnd`, `RowVersion` |
| `OtpCode` | `OtpId` | `UserId`, `ChallengeId`, `CodeHash`, `Purpose`, `CreatedAt`, `ExpiresAt`, `IsUsed`, `FailedAttempts` |
| `RefreshToken` | `TokenId` | `UserId`, `SessionRootTokenId`, `SessionVersion`, `TokenHash`, `ExpiresAt`, `RotatedAt`, `RevokedAt` |

The entities inherit the repository's integer `Id` property, which EF maps to the
diagram's primary-key column names. A user has many OTP codes and refresh tokens.
Both child tables have required, indexed foreign keys; deleting a user cascades
to its codes and tokens. Normalized emails, challenge identifiers, and refresh-token
hashes have unique indexes. `RevokedAt` is nullable. `Name` is nullable for legacy
accounts whose names are unknown. Registration always requires a name.
Timestamps use `DateTimeOffset` in UTC. `Role` remains a required string;
registration always sets it to `User`.

Entity configurations live under `Infrastructure/Persistence/Configurations`
and are discovered by `AppDbContext`. The generic repositories can operate on all
three entities.

With the .NET 10 `dotnet-ef` tool installed and the target connection configured,
apply the migration from the repository root:

```powershell
dotnet ef database update --project IdentityService/IdentityService.csproj --startup-project IdentityService/IdentityService.csproj
```

For Docker's SQL Server, the host connection uses `localhost,1433`, rather than
the Compose-only hostname `sqlserver`. Set the connection string through user
secrets or the environment before running the command. Migrations are applied
explicitly; starting the API does not change the database schema.

Apply this change in a maintenance window: stop the old service, take a backup,
apply pending migrations, configure SMTP, OTP, and JWT settings, and start the new service.
The old and new OTP schemas are not compatible with a rolling deployment.
The migration backfills ASCII legacy emails with the same trim/uppercase rules.
It stops atomically for collisions or legacy emails requiring explicit Unicode
normalization; resolve those records and retry. It never merges or deletes accounts.
Legacy OTP values are overwritten and marked used, with new unique challenge IDs.

The down migration supports schema rollback before named accounts are registered.
It refuses to discard stored names after registration has been used; use a forward
fix or the pre-deployment backup in that case. Invalidated OTPs are not restored.

## Gmail setup

`appsettings.Development.json` contains examples in `SmtpSettings` and
`OtpSettings`. Replace the sender address, username, and Gmail app-password
placeholder locally. Gmail uses `smtp.gmail.com:587` with mandatory `StartTls`.
Enable two-step verification and create an app password if your Gmail account
supports it; use that app password instead of the account's normal password.
TLS certificate validation remains enabled.

Generate a separate OTP protection key locally:

```powershell
[Convert]::ToBase64String([System.Security.Cryptography.RandomNumberGenerator]::GetBytes(32))
```

Put it in `OtpSettings:ProtectionKey`. Keep actual credentials and this key out
of commits. User secrets are available through the project's existing
`UserSecretsId`; environment overrides are `SmtpSettings__Password` and
`OtpSettings__ProtectionKey`. Docker Compose reads the variables documented in
the root `.env.example`; copy it to the ignored `.env` and replace placeholders.
Changing the OTP protection key invalidates outstanding codes.

Options are validated on startup without contacting Gmail. Missing credentials,
unchanged placeholders, invalid protection keys, or unsupported TLS modes stop
startup with a generic configuration error.

## Send email from another feature

Inject `IEmailSender` into any IdentityService handler or service and supply the
recipient, subject, and plain-text body:

```csharp
var status = await emailSender.SendAsync(
    "recipient@example.com",
    "Your FitnessApp notification",
    "Your requested action is complete.",
    cancellationToken);
```

Feature handlers own their email content; the sender only handles SMTP delivery.
Registration builds its verification email in `SendVerificationEmailHandler`.
`EmailDeliveryStatus.Accepted` means SMTP accepted the message. `Rejected`,
`Unavailable`, and `Unknown` describe delivery failures; request cancellation
propagates. The existing `AddIdentityRegistration` setup registers the sender.

## Registration

`POST /api/identity/register` accepts only name, email, and password as registration
inputs. See `IdentityService.http` for requests. Name is trimmed and limited to
1–100 characters; email is trimmed, syntax-checked, and limited to 256 characters.
Passwords must contain 15–128 characters and are preserved exactly, including spaces.
These limits are implementation policy where SCRUM-12 is silent.

The endpoint injects MediatR `ISender` and sends `RegisterUserCommand`.
`RegisterUserOrchestrator` handles that request through `IRequestHandler`,
explicitly invokes FluentValidation, and dispatches `EmailExistsQuery`,
`CreateUserCommand`, `CreateEmailVerificationOtpCommand`, and
`SendVerificationEmailCommand` through the same sender. MediatR 12.4.1 scans
the service assembly for handlers; the endpoint does not inject the orchestrator.
Every command handler wraps its action in `IUnitOfWork.ExecuteAsync`.
The orchestrator adds an outer call around the account and OTP commands, so those
nested handlers stage changes and commit together. The email command runs in a
separate `ExecuteAsync` call after the account/challenge transaction has committed.
Its SQL rollback cannot undo an external SMTP submission.
Handlers obtain cached repositories through the UOW. Microsoft `PasswordHasher<ApplicationUser>`
hashes passwords. Email normalization uses `Trim().ToUpperInvariant()`, and the
named SQL unique index also protects concurrent requests.

The account starts with `User` role and an unverified email. Its six-digit code
expires after ten minutes by default. Only an HMAC-SHA-256 hash bound to the
challenge ID and purpose is persisted; plaintext exists only in request memory
and the outgoing email. The later verification flow must enforce expiration,
`IsUsed`, and the configured five-attempt limit.

Account and challenge commit together before the email command calls MailKit.
There is no FitnessProfile creation/call, authentication token response, event
publication, RabbitMQ dependency, or durable email queue in this flow.

| Outcome | HTTP and envelope status |
| --- | --- |
| Account created and SMTP accepts the email | 201 |
| Invalid/missing input or malformed JSON | 400 |
| Duplicate email, including a concurrent registration | 409 |
| Rate limit exceeded | 429 |
| Email rejection, provider unavailability, or uncertain submission | 503 |
| Unexpected persistence/application failure | 500 |

SMTP acceptance means submission, not guaranteed inbox delivery. A 503 states
that the account was created and remains unverified. Timeouts during submission
are uncertain: an arriving code remains usable until expiration. The service
does not automatically retry email.

Cancellation before commit rolls back when observed; cancellation after commit
does not remove the account. A process failure between commit and sending can
leave an unverified account without email. A lost commit acknowledgment is also
uncertain; retrying registration returns 409 if the account exists. Retrying after
email failure does not create a second account or send another code.

Verification and resend remain separate features. Resend must be available before
public registration launches; it must generate a fresh code and invalidate previous
challenges because persisted hashes cannot recover plaintext codes. Delivery is
not guaranteed or exactly once.

## Login and account protection

`POST /api/identity/login` accepts email and password over HTTPS. A verified, enabled
account receives a 200 envelope with `data.accessToken` and `Cache-Control: no-store`.
The host-only `refresh_token` cookie uses `HttpOnly`, `Secure`, `SameSite=Lax`, and
`Path=/api/identity`. Its expiry matches the stored expiry. This assumes a same-site client
deployment. The database stores the SHA-256 hash of the opaque token.

Unknown email, wrong password, and unverified, disabled, or locked accounts receive
the same 401 response. The fifth wrong password locks an eligible account for
15 minutes. Attempts during lockout preserve its deadline. A successful login clears
the count. The endpoint also allows 20 requests per IP per minute per service instance.

`LoginOrchestrator` dispatches MediatR queries to load the account and verify the
password, then commands to update login attempts and issue tokens. A row-version
conflict restarts the account query and password check. The successful account
update and refresh-token insert commit in the same transaction.

Apply `AddLoginProtection` before deploying the new service. It enables existing users,
preserves their email-verification state, starts their failure count at zero, and adds
a SQL Server row version. It hashes and revokes legacy refresh tokens. Rolling back also
revokes existing sessions because their plaintext tokens cannot be recovered.

### Configure signing and export the public PEM

Configure a stable RSA private PEM (at least 2048 bits) in IdentityService's secret store
under `JwtSettings:PrivateKeyPem`. Use the same key across IdentityService replicas
and restarts. For a **new** local development installation, run this once from the
repository root in PowerShell 7.4 or later. Skip key generation when a signer already exists.

```powershell
$rsa = [System.Security.Cryptography.RSA]::Create(3072)
try {
    @{ 'JwtSettings:PrivateKeyPem' = $rsa.ExportPkcs8PrivateKeyPem() } |
        ConvertTo-Json -Compress | dotnet user-secrets set --project IdentityService
} finally {
    $rsa.Dispose()
}
```

Export from the configured signer. The command creates a new PEM file and exits.
Choose a fresh output filename for a later export.

```powershell
New-Item -ItemType Directory -Force artifacts | Out-Null
$publicPath = Join-Path (Get-Location) 'artifacts/identity-public.pem'
dotnet run --project IdentityService --launch-profile https -- --export-public-key $publicPath
$publicPem = [IO.File]::ReadAllText($publicPath)
foreach ($service in 'FitnessProfileService','NutritionService','PaymentService','ProgressService','TrainingService') {
    @{ 'JwtSettings:PublicKeyPem' = $publicPem } | ConvertTo-Json -Compress |
        dotnet user-secrets set --project "$service/$service.csproj"
}
```

For Docker Compose, provide multiline `JWT_PRIVATE_KEY_PEM` and `JWT_PUBLIC_KEY_PEM`
in the deployment environment or the ignored local `.env`. Use single quotes around
multiline values in `.env` to preserve the PEM boundaries and line breaks. Compose
passes the private PEM exclusively to IdentityService. Supply matching `JWT_ISSUER`,
`JWT_AUDIENCE`, and `JWT_KEY_ID` to all services when overriding their defaults.
For local browser testing, trust the development certificate and run the HTTPS profile.
Use HTTPS ingress when running the browser client against Compose.

```powershell
dotnet dev-certs https --trust
dotnet ef database update --project IdentityService/IdentityService.csproj --startup-project IdentityService/IdentityService.csproj -- --environment Development
dotnet run --project IdentityService --launch-profile https
```

### Use access tokens in the other APIs

Send `Authorization: Bearer <accessToken>` with each protected request. Fitness Profile,
Nutrition, Payment, Progress, and Training use the shared `AddAccessTokenAuthentication`
registration and ASP.NET Core JWT bearer middleware. Their sample `/weatherforecast`
endpoints require authentication. Each API checks the RS256 signature with its local
public key, the configured issuer and audience, and expiry with zero clock skew.
Keep clocks synchronized. Role policies use the stored account's `role` claim and
the principal name is the user ID from `sub`. Validation works while IdentityService is offline.
[ASP.NET Core JWT bearer documentation](https://learn.microsoft.com/en-us/aspnet/core/security/authentication/configure-jwt-bearer-authentication?view=aspnetcore-10.0)

The defaults are `FitnessApp.Identity` for issuer, `FitnessApp.Api` for audience, and
`fitness-1` for key ID. Set `JwtSettings:AccessLifetimeMinutes` (default 15) and
`JwtSettings:RefreshLifetimeDays` (default 7) on IdentityService.

### Refresh or end the current session

Apply the `AddRefreshSessions` migration before deploying these endpoints. Existing
tokens keep their expiry and revocation state. Each starts its own session using its
original token ID, and replacements refer back to that ID. Schema rollback drops
the session links and rotation timestamps. Used tokens stay revoked.

- `POST /api/identity/refresh` takes the `refresh_token` cookie over HTTPS. It works
  after the access JWT expires. A successful call returns a fresh `data.accessToken`
  and replaces the cookie. The replacement keeps the original session deadline.
  Missing, invalid, expired, revoked, and used tokens receive the same `401`.
- `POST /api/identity/logout` revokes the session identified by the cookie, clears
  the cookie, and returns `204`. Other sessions stay signed in. Previously issued
  access JWTs remain valid until their own expiration.

Reusing a rotated token revokes the active refresh token in that session and records
a warning with the user ID and session ID. Keep one refresh request in flight per
session. If refresh returns `401`, prompt for sign-in.


## Verification

```powershell
dotnet build FitnessApp-team3.slnx
dotnet ef migrations has-pending-model-changes --project IdentityService/IdentityService.csproj --startup-project IdentityService/IdentityService.csproj
```
