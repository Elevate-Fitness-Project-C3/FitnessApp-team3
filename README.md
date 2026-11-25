# FitnessApp-team3

## Run with Docker Compose

Use Docker Desktop with Linux containers. SQL Server runs the Developer edition
for local development. Allow enough Docker memory for SQL Server (at least 2 GB
for SQL Server alone), RabbitMQ, and the .NET services.

From the repository root:

```powershell
Copy-Item .env.example .env.local
# Configure database, RabbitMQ, SMTP, OTP, and JWT values in .env.local.
docker compose --env-file .env.local up --build -d
docker compose --env-file .env.local ps -a
```

The default stack starts all six APIs, including Payment, SQL Server, RabbitMQ,
and a one-time database initializer.

| Service | Local address | Database |
| --- | --- | --- |
| Identity | http://localhost:5002 | FitnessIdentity |
| Fitness Profile | http://localhost:5007 | FitnessProfile |
| Training | http://localhost:5296 | FitnessTraining |
| Progress | http://localhost:5043 | FitnessProgress |
| Nutrition | http://localhost:5041 | FitnessNutrition |
| Payment | http://localhost:5035 | FitnessPayment |
| SQL Server | localhost,11433 | Login: sa; password from .env.local |
| RabbitMQ AMQP | localhost:5673 | Credentials from .env.local |
| RabbitMQ management | http://localhost:15673 | Credentials from .env.local |

`docker-compose.override.yml` is loaded automatically for local development. It
contains all service environment settings, enables Development, and publishes
ports only on localhost. Container traffic uses
HTTP on port 8080, so no local HTTPS certificate mounts are needed. Identity
Swagger UI is at http://localhost:5002/swagger. The other API scaffolds currently
expose `/openapi/v1.json` and a bearer-protected `/weatherforecast`. Configure the
signing and verification keys before starting the APIs. [Access token verification
and key setup](docs/access-token-verification.md)

Inside the Compose network, use service names and container ports, for example
`http://fitnessprofileservice:8080`, `sqlserver,1433`, and `rabbitmq:5672`.
Host ports in the table are for tools running outside Docker.

## Persistence and messaging

Each API receives `ConnectionStrings__DefaultConnection` pointing to its own
database on the local SQL Server instance. The initializer waits for SQL Server
to become healthy and creates missing databases without dropping existing data.
It also prepares the Payment database. APIs wait for initialization to
finish. Identity, Training, and Progress also wait for RabbitMQ readiness.

Identity, Training, and Progress receive `RabbitMq__Host`, `RabbitMq__Username`,
and `RabbitMq__Password`. Identity publishes `UserCreated` via a SQL outbox.
Apply the Identity schema migrations as described in
[IdentityService/README.md](IdentityService/README.md). The database initializer
creates databases. Identity migrations are applied separately.

SQL Server and RabbitMQ data use named volumes and survive `docker compose down`.
The local stack uses the SQL Server `sa` account; use restricted service accounts
and managed credentials for a deployed environment. Changing passwords in `.env.local`
does not rotate credentials already stored in existing data volumes.

## Useful commands

```powershell
docker compose --env-file .env.local config --quiet
docker compose --env-file .env.local logs -f identityservice
docker compose --env-file .env.local logs database-init
docker compose --env-file .env.local down
```

The last command stops all services and preserves stored data.
In Visual Studio, configure `.env.local` first, then select Docker Compose as the startup
project. All six APIs are configured for debugging, including Payment.

The base Compose file defines builds, dependencies, storage, and health checks.
Running with only `docker-compose.yml` excludes all service environment settings
and published ports. A deployment must supply its own override with the required
environment settings and credentials, plus ingress/TLS configuration.
