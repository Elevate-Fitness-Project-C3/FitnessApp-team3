# IdentityService

The identity feature layout follows [exam-system-team-5](https://github.com/Elevate-Exam-Projects-c3/exam-system-team-5/tree/e6af33a69e3725bdc1fde5b1aea4d0e0247d3562/Features/Identity/Register).
Repository placement and contracts follow [LMS Mini Version](https://github.com/Amr-shawky/LMS---Mini-Version/tree/1a63882d1743ecdc5ed005b4dcf14ce129a5836a/Infrastructure/Repositories).

## Structure

- `Common/Middleware`: global exception middleware and shared exceptions.
- `Common/Results`: `Result` and `Result<T>`.
- `Domain/Common`: `BaseEntity` with an integer `Id`, matching the LMS contract.
- `Domain/Entities/Identity`: identity entity scaffold.
- `Domain/Repositories`: `IGeneralRepository<T>` and `IUnitOfWork`.
- `Features/Identity`: ForgotPassword, Login, Logout, RefreshTokens, Register,
  VerifyEmailOtp, and shared ViewModels folders.
- `Features/Shared`: HTTP response envelope and shared DTO scaffold.
- `Infrastructure/Persistence`: `AppDbContext` and entity configurations.
- `Infrastructure/Repositories`: `GeneralRepository<T>` and `UnitOfWork`.
- `Infrastructure/DependencyInjection.cs`: scoped persistence registrations.
- `Migrations`: migration scaffold.

`Features/Identity/Register` contains `Commands`, `Controllers`, `DTOs`,
`Events`, `Handlers`, `Orchestrators`, `Queries`, and `Validators`, matching
the reference. Other operations retain the DTO, event, and view model folders
present in that reference. Empty folders contain `.gitkeep` files.

Feature business logic, including registration and OTP delivery, is still a scaffold.

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
The document has no operations until feature modules define routes.

The built-in global rate limiter allows 100 requests per minute per client IP,
with no queue. Excess requests return HTTP 429. Limits are held in memory per
service instance. Behind a reverse proxy, configure trusted forwarded headers
before rate limiting so clients are identified by their actual IP.

## Base file behavior

`GeneralRepository<T>` supports queries, CRUD, explicit saves, and `SaveInclude`
for selected scalar property updates. Partial updates support detached objects
and objects already tracked by the context. Write methods do not save automatically.

`UnitOfWork.ExecuteAsync` saves and commits at the outermost operation.
Exceptions roll back the transaction and clear tracked entities. A nested failure
prevents the outer transaction from committing even when the caller catches it.

`AddSavePointAsync` saves pending changes before creating the savepoint.
`RollbackToSavePointAsync` clears tracked entities; reload entities before continuing.

The repository and unit of work share one scoped `AppDbContext`.
The unit of work owns its transaction; the service scope owns the context.

`GlobalExceptionMiddleware` is registered in `Program.cs`. It returns a
`RequestResponse<T>` JSON envelope for validation (400), unauthorized (401),
not found (404), conflict (409), gone (410), and unexpected (500) errors.
Unexpected errors are logged and return a generic message. Request cancellation
propagates, and responses that have started are not overwritten.

## Configure persistence

Install the EF Core 10 database provider selected for this service. In `Program.cs`,
import `IdentityService.Infrastructure` and call
`builder.Services.AddIdentityPersistence(options => ...)`, configuring the
provider inside the callback.

Add identity entities under `Domain/Entities/Identity` and their
`IEntityTypeConfiguration<T>` classes under
`Infrastructure/Persistence/Configurations`. `AppDbContext` discovers
configurations in the service assembly.

No database provider, connection string, schema migration, or registration
endpoint is configured yet.

## Verification

```powershell
dotnet build IdentityService/IdentityService.csproj
```
