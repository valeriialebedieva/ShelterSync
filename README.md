# ShelterSync

ShelterSync — Pet Adoption Management System

An ASP.NET Core web application that helps animal shelters manage pets and handle adoption requests. Browse available pets, submit adoption forms, and manage the shelter inventory through a staff login.

## Tech Stack

- **Framework:** ASP.NET Core 8 MVC + Blazor
- **ORM:** Entity Framework Core 8
- **Database:** PostgreSQL 16
- **Database Provider:** Npgsql
- **Deployment:** Docker (Render)

---

## Quick Start (Local)

If you already have .NET and Docker installed:

```bash
git clone <repository-url>
cd ShelterSync
docker compose up -d
cd ShelterSync
dotnet restore
dotnet run
```

Open **http://localhost:5023** in your browser.

---

## Run Locally — Step by Step

### Step 0 — Check prerequisites

Install these tools before you begin:

| Tool | Version | Download |
|------|---------|----------|
| **.NET SDK** | 8.0 or newer | https://dotnet.microsoft.com/download |
| **Docker Desktop** | Latest | https://docs.docker.com/get-docker/ |
| **Git** | Any recent | https://git-scm.com/downloads |

Verify they are installed:

```bash
dotnet --version    # Should print 8.x or 9.x
docker --version    # Should print Docker version
git --version
```

**Optional (recommended):** install EF Core CLI for creating migrations:

```bash
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"   # add to ~/.zshrc to make permanent
dotnet ef --version
```

> **Note:** The app applies migrations automatically on startup, so `dotnet ef` is only required if you change database models.

---

### Step 1 — Clone the repository

```bash
git clone <repository-url>
cd ShelterSync
```

Confirm you are in the correct folder. You should see:

```
ShelterSync/
├── docker-compose.yml
├── Dockerfile
├── ShelterSync.sln
└── ShelterSync/          ← ASP.NET project folder
    ├── Program.cs
    ├── appsettings.json
    └── ...
```

---

### Step 2 — Start Docker Desktop

1. Open **Docker Desktop** on your machine
2. Wait until it shows **Docker is running**

Without this step, the database container will not start.

---

### Step 3 — Start PostgreSQL

From the **repository root** (where `docker-compose.yml` is located):

```bash
docker compose up -d
```

**Expected output:**

```
✔ Container sheltersync-db  Started
```

Verify the database is running:

```bash
docker compose ps
```

**Expected output:**

| NAME | STATUS |
|------|--------|
| sheltersync-db | running |

**Database credentials** (already set in `ShelterSync/appsettings.json`):

| Setting  | Value             |
|----------|-------------------|
| Host     | `localhost`       |
| Port     | `5432`            |
| Database | `sheltersync`     |
| Username | `sheltersync`     |
| Password | `sheltersync_dev` |

If port 5432 is already in use by another PostgreSQL install, stop that service first or change the port in `docker-compose.yml` and update `appsettings.json` to match.

---

### Step 4 — Restore NuGet packages

```bash
cd ShelterSync
dotnet restore
```

**Expected output:**

```
Restore succeeded.
```

---

### Step 5 — Apply database migrations (optional)

Migrations run automatically when the app starts, but you can apply them manually first:

```bash
dotnet ef database update
```

**Expected output:**

```
Applying migration '20260530200226_AddPetsTable'.
Done.
```

This creates the `Pets` table and inserts 6 sample pets (Max, Luna, Charlie, etc.).

If `dotnet ef` is not installed, skip this step — the app will migrate on startup.

---

### Step 6 — Run the application

```bash
dotnet run
```

Or from the repository root:

```bash
dotnet run --project ShelterSync/ShelterSync.csproj
```

**Expected output:**

```
Now listening on: http://localhost:5023
Application started. Press Ctrl+C to shut down.
```

Keep this terminal open while using the app.

#### Auto-reload during development

```bash
dotnet watch run
```

The app restarts automatically when you edit code.

#### HTTPS locally (optional)

```bash
dotnet run --launch-profile https
```

Then open https://localhost:7077

---

### Step 7 — Open the app in your browser

| URL | Page |
|-----|------|
| http://localhost:5023 | Home — pet gallery |
| http://localhost:5023/Pet | All pets (with search) |
| http://localhost:5023/Account/Login | Staff sign in |
| http://localhost:5023/admin | Admin panel (Blazor, requires login) |

You should see sample pets on the home page after a successful first run.

---

### Step 8 — Sign in (demo mode)

Login is demo-only — no real password database:

| Field | Value |
|-------|-------|
| **Username** | Any non-empty value |
| **Password** | Any value with at least 6 characters |
| **Admin access** | Use username `admin` |

After signing in you can:

- Add, edit, and delete pets at `/Pet`
- Access the admin panel at `/admin`

---

### Step 9 — Submit a test adoption request

1. Go to http://localhost:5023
2. Click **Request Adoption** on any pet
3. Fill in the form and submit

Adoption requests are saved to a local JSON file (not PostgreSQL):

```
ShelterSync/data/adoption_requests.json
```

The `data/` folder is created automatically on the first submission.

---

### Step 10 — Stop everything when done

```bash
# Stop the app
Ctrl+C   (in the terminal running dotnet run)

# Stop the database (from repo root)
cd ..
docker compose down

# Stop database AND delete all stored data
docker compose down -v
```

---

## Local development checklist

Use this checklist if something does not work:

- [ ] Docker Desktop is running
- [ ] `docker compose ps` shows `sheltersync-db` as **running**
- [ ] You ran `dotnet restore` inside the `ShelterSync/` folder
- [ ] Terminal shows `Now listening on: http://localhost:5023`
- [ ] You are opening **http://localhost:5023** (not a random port)
- [ ] The terminal running `dotnet run` is still open

---

## Troubleshooting (Local)

### `Connection refused` on port 5432

PostgreSQL is not running.

```bash
# From repo root
docker compose up -d
docker compose ps
```

### `dotnet ef` command not found

```bash
dotnet tool install --global dotnet-ef
export PATH="$PATH:$HOME/.dotnet/tools"
```

### `relation "Pets" does not exist`

Migrations were not applied. Run:

```bash
cd ShelterSync
dotnet ef database update
```

Or restart the app — it runs `Database.Migrate()` on startup.

### Docker not installed

Install [Docker Desktop](https://docs.docker.com/get-docker/), **or** install PostgreSQL directly and update the connection string in `ShelterSync/appsettings.json`.

### Port 5023 already in use

Run on a different port:

```bash
dotnet run --urls "http://localhost:5050"
```

### `docker: command not found`

Install Docker Desktop and restart your terminal.

### Home page shows no pets

Ensure migrations ran successfully:

```bash
cd ShelterSync
dotnet ef database update
```

Then restart the app.

---

## Deploy to Render

### Step 1 — Push code to GitHub

Render deploys from Git. Commit and push your repo.

### Step 2 — Create a PostgreSQL database on Render

1. Render Dashboard → **New +** → **PostgreSQL**
2. Name: `sheltersync-db`
3. Copy the **Internal Database URL** after it is created

### Step 3 — Create a Web Service

1. **New +** → **Web Service** → connect your GitHub repo
2. Use these settings:

| Setting | Value |
|---------|--------|
| **Language** | **Docker** (not Node) |
| **Dockerfile Path** | `./Dockerfile` |
| **Docker Context** | `.` |
| **Branch** | `main` |

### Step 4 — Add environment variables

| Key | Value |
|-----|--------|
| `ASPNETCORE_ENVIRONMENT` | `Production` |
| `ConnectionStrings__DefaultConnection` | Your Npgsql connection string (see below) |

Convert Render's internal URL from:

```
postgresql://sheltersync:PASSWORD@dpg-xxxxx-a/sheltersync
```

To:

```
Host=dpg-xxxxx-a;Port=5432;Database=sheltersync;Username=sheltersync;Password=PASSWORD;SSL Mode=Require;Trust Server Certificate=true
```

### Step 5 — Deploy

Click **Create Web Service**. Render builds the Docker image and deploys. Migrations run automatically on startup.

---

## Adoption requests storage

Adoption form submissions are saved by `AdoptionService` to a **local JSON file**, not PostgreSQL:

| Environment | File path |
|-------------|-----------|
| Local | `ShelterSync/data/adoption_requests.json` |
| Render (Docker) | `/app/data/adoption_requests.json` (inside the container) |

> **Note:** On Render, this file is stored on ephemeral disk and may be **lost on redeploy or restart**. For production, adoption requests should be moved to PostgreSQL.

---

## Useful commands

```bash
# Check if the database container is running
docker compose ps

# View database logs
docker compose logs db

# Build without running
cd ShelterSync && dotnet build

# Create a new migration after changing models
cd ShelterSync
dotnet ef migrations add <MigrationName>

# Apply pending migrations manually
dotnet ef database update

# Revert the last unapplied migration
dotnet ef migrations remove
```

---

## Project structure

```
ShelterSync/
├── docker-compose.yml       # Local PostgreSQL
├── Dockerfile               # Render / production deployment
├── ShelterSync.sln
└── ShelterSync/
    ├── Program.cs           # App entry point, DI, auto-migrations
    ├── appsettings.json     # Connection string
    ├── Controllers/         # MVC controllers
    ├── Views/               # Razor views
    ├── Models/              # Pet, AdoptionRequest, etc.
    ├── Services/            # PetService, AdoptionService
    ├── Data/                # ShelterSyncDbContext
    ├── Migrations/          # EF Core migrations
    ├── Pages/               # Blazor pages (admin)
    └── wwwroot/             # CSS, JS, static files
```

---

## Adding a new database model

1. Create your model class in `Models/`
2. Add a `DbSet<YourModel>` to `Data/ShelterSyncDbContext.cs`
3. Run `dotnet ef migrations add <DescriptiveName>` from the `ShelterSync/` folder
4. Run `dotnet ef database update` (or restart the app)

---

## License

© 2026 ShelterSync
