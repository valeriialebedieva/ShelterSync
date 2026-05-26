using Microsoft.EntityFrameworkCore;

namespace ShelterSync.Data;

/// <summary>
/// Entity Framework Core database context for ShelterSync.
/// Add DbSet properties here as domain models are created.
/// </summary>
public class ShelterSyncDbContext : DbContext
{
    public ShelterSyncDbContext(DbContextOptions<ShdoelterSyncDbContext> options)
        : base(options) { }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
