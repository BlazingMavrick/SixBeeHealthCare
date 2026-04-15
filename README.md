# SixBee Healthcare – Tech Test

An ASP.NET Core 8 MVC web application for managing patient appointments, with cookie-based admin authentication and a SQL Server backend.

## Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/) (for the SQL Server container)

---

## Solution Structure

```
SixBeeHealthCare.slnx
├── SixBeeHealthCare/          # ASP.NET Core MVC web app
└── SixBeeHealthCare.Tests/    # NUnit integration tests
```

---

## Option 1 – Run with Docker Compose (full stack)

This starts both the SQL Server database and the web app together.

```bash
cd SixBeeHealthCare
docker-compose up --build
```

The app will be available at **http://localhost:8080**

---

## Option 2 – Run locally (database in Docker, app in Visual Studio / CLI)

**Step 1 – Start the database container**

```bash
cd SixBeeHealthCare
docker-compose up -d db
```

Wait until the container is healthy (roughly 30 seconds). You can check with:

```bash
docker-compose ps
```

Look for `healthy` next to the `db` service before proceeding.

**Step 2 – Run the web app**

Either open `SixBeeHealthCare.slnx` in Visual Studio and press F5, or run from the CLI:

```bash
dotnet run --project SixBeeHealthCare/SixBeeHealthCare.Web.csproj
```

The app creates and seeds the database schema automatically on first run.

---

## Admin Login

A default admin account is seeded on startup:

| Field    | Value                  |
|----------|------------------------|
| Email    | admin@sixbee.nhs.uk    |
| Password | Admin1234!             |

---

## Running the Tests

The tests are integration tests that run against a real SQL Server instance. The database container must be running before executing them.

**Step 1 – Start the database container** (if not already running)

```bash
cd SixBeeHealthCare
docker-compose up -d db
```

Wait for the container to reach a `healthy` state.

**Step 2 – Run the tests**

From Visual Studio, use the Test Explorer. Or from the CLI at the repo root:

```bash
dotnet test SixBeeHealthCare.Tests/SixBeeHealthCare.Tests.csproj
```

The test fixture creates an isolated database per test run (with a random name) and drops it on teardown, so tests are safe to run repeatedly alongside the running application.

### Custom connection string

By default the tests connect to `localhost,1433` with the `sa` account. If your SQL Server is on a different host or port, set the `SIXBEE_TEST_CONNECTION_STRING` environment variable before running:

```bash
$env:SIXBEE_TEST_CONNECTION_STRING = "Server=myhost,1433;Database=master;User Id=sa;Password=SixBee_Dev_2026!;TrustServerCertificate=True"
dotnet test SixBeeHealthCare.Tests/SixBeeHealthCare.Tests.csproj
```
