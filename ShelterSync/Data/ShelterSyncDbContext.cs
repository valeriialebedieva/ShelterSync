using Microsoft.EntityFrameworkCore;
using ShelterSync.Models;

namespace ShelterSync.Data;

/// <summary>
/// Entity Framework Core database context for ShelterSync.
/// Add DbSet properties here as domain models are created.
/// </summary>
public class ShelterSyncDbContext : DbContext
{
    public ShelterSyncDbContext(DbContextOptions<ShelterSyncDbContext> options)
        : base(options) { }

    public DbSet<Pet> Pets { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
    }
}
