# FitnessApp-team3

## Run with Docker Compose

Use Docker Desktop with Linux containers. SQL Server runs the Developer edition
for local development. Allow enough Docker memory for SQL Server (at least 2 GB
for SQL Server alone), RabbitMQ, and the .NET services.

From the repository root:

```powershell
Copy-Item .env.example .env
# Edit .env and replace both example passwords.
docker compose up --build -d
docker compose ps -a
```

The default stack starts the five core APIs, SQL Server, RabbitMQ, and a one-time
database initializer. Payment is optional:

```powershell
docker compose --profile payment up --build -d
```

| Service | Local address | Database |
| --- | --- | --- |
| Identity | http://localhost:5002 | FitnessIdentity |
| Fitness Profile | http://localhost:5007 | FitnessProfile |
| Training | http://localhost:5296 | FitnessTraining |
| Progress | http://localhost:5043 | FitnessProgress |
| Nutrition | http://localhost:5041 | FitnessNutrition |
| Payment (optional) | http://localhost:5035 | FitnessPayment |
| SQL Server | localhost,1433 | Login: sa; password from .env |
| RabbitMQ AMQP | localhost:5672 | Credentials from .env |
| RabbitMQ management | http://localhost:15672 | Credentials from .env |

`docker-compose.override.yml` is loaded automatically for local development. It
enables Development and publishes ports only on localhost. Container traffic uses
HTTP on port 8080, so no local HTTPS certificate mounts are needed. Identity
Swagger UI is at http://localhost:5002/swagger. The other API scaffolds currently
expose `/openapi/v1.json` and `/weatherforecast`.

Inside the Compose network, use service names and container ports, for example
`http://fitnessprofileservice:8080`, `sqlserver,1433`, and `rabbitmq:5672`.
Host ports in the table are for tools running outside Docker.

## Persistence and messaging

Each API receives `ConnectionStrings__DefaultConnection` pointing to its own
database on the local SQL Server instance. The initializer waits for SQL Server
to become healthy and creates missing databases without dropping existing data.
It also prepares the optional Payment database. APIs wait for initialization to
finish; Training and Progress additionally wait for RabbitMQ readiness.

Training and Progress receive `RabbitMq__Host`, `RabbitMq__Username`, and
`RabbitMq__Password` for the completed-workout event flow. These are configuration
keys for the future application integrations: the current scaffolds do not yet
register a SQL Server EF provider, run schema migrations, or publish/consume events.
Database creation alone does not create application tables.

SQL Server and RabbitMQ data use named volumes and survive `docker compose down`.
The local stack uses the SQL Server `sa` account; use restricted service accounts
and managed credentials for a deployed environment. Changing passwords in `.env`
does not rotate credentials already stored in existing data volumes.

## Useful commands

```powershell
docker compose config --quiet
docker compose logs -f identityservice
docker compose logs database-init
docker compose --profile payment down
```

The last command stops both core and optional services and preserves stored data.
In Visual Studio, copy/edit `.env` first, then select Docker Compose as the startup
project. The five core APIs are configured for debugging.

The base Compose file keeps infrastructure ports private and uses the .NET
Production default. To use it without the development override:

```powershell
docker compose -f docker-compose.yml up --build -d
```

This base configuration is a starting point; a deployment still needs its own
ingress/TLS configuration and credentials.
