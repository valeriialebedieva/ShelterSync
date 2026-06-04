# ShelterSync

ShelterSync - Pet Adoption Management System

## About

> ShelterSync is a comprehensive .NET Blazor web application designed to help animal shelters manage their pet inventory and facilitate pet adoptions. The application provides a user-friendly interface for browsing available pets, managing shelter operations, and connecting potential adopters with their perfect companion.

## Favorite Quote

> "The greatness of a nation can be judged by the way its animals are treated." – Mahatma Gandhi

## Tech Stack

- **Framework:** ASP.NET Core 8 MVC
- **ORM:** Entity Framework Core 8
- **Database:** PostgreSQL 16 (via Docker)
- **Database Provider:** Npgsql

## Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (or newer)
- [Docker](https://docs.docker.com/get-docker/)

### Setup

1. **Clone the repository:**
   ```bash
   git clone <repository-url>
   cd ShelterSync
   ```

2. **Start the database:**
   ```bash
   docker compose up -d
   ```
   This starts a PostgreSQL 16 container on `localhost:5023` with the following credentials:
   | Setting  | Value             |
   |----------|-------------------|
   | Host     | `localhost`       |
   | Port     | `5023`            |
   | Database | `sheltersync`     |
   | Username | `sheltersync`     |
   | Password | `sheltersync_dev` |

3. **Apply database migrations:**
   ```bash
   cd ShelterSync
   dotnet ef database update
   ```

4. **Run the application:**
   ```bash
   dotnet run
   ```

### Useful Commands

```bash
# Check if the database container is running
docker compose ps

# Stop the database
docker compose down

# Stop the database AND delete all data
docker compose down -v

# Create a new migration after changing models
dotnet ef migrations add <MigrationName>

# Apply pending migrations
dotnet ef database update

# Revert the last migration (if not applied)
dotnet ef migrations remove
```

## Project Structure

```
ShelterSync/
├── docker-compose.yml           # PostgreSQL container definition
├── ShelterSync.sln
└── ShelterSync/
    ├── ShelterSync.csproj
    ├── Program.cs               # App entry point + DB registration
    ├── appsettings.json         # Connection string config
    ├── Data/
    │   └── ShelterSyncDbContext.cs  # EF Core DbContext
    ├── Models/                  # Domain models (add DbSets to DbContext)
    ├── Migrations/              # Auto-generated EF Core migrations
    ├── Controllers/
    ├── Views/
    └── wwwroot/
```

## Adding a New Model

1. Create your model class in `Models/`
2. Add a `DbSet<YourModel>` property to `Data/ShelterSyncDbContext.cs`
3. Run `dotnet ef migrations add <DescriptiveName>`
4. Run `dotnet ef database update`
