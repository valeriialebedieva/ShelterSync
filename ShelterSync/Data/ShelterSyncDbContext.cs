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

        modelBuilder.Entity<Pet>().HasData(
            new Pet
            {
                Id = 1,
                Name = "Max",
                Species = "Dog",
                Breed = "Golden Retriever",
                Age = 3,
                Notes = "Friendly and energetic, loves to play fetch. Great with kids!",
                ImagePath = "https://images.dog.ceo/breeds/retriever-golden/n02099601_3004.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = true
            },
            new Pet
            {
                Id = 2,
                Name = "Luna",
                Species = "Cat",
                Breed = "Siamese",
                Age = 2,
                Notes = "Playful and affectionate. Loves attention and interactive toys.",
                ImagePath = "https://cdn2.thecatapi.com/images/N7rlRo9Zi.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = false
            },
            new Pet
            {
                Id = 3,
                Name = "Charlie",
                Species = "Dog",
                Breed = "Labrador Mix",
                Age = 5,
                Notes = "Calm and gentle companion. Perfect for families looking for a loyal friend.",
                ImagePath = "https://images.dog.ceo/breeds/labrador/john_walker_000.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = false
            },
            new Pet
            {
                Id = 4,
                Name = "Whiskers",
                Species = "Cat",
                Breed = "Tabby",
                Age = 1,
                Notes = "Young and curious kitten. Still learning about the world around her.",
                ImagePath = "https://cdn2.thecatapi.com/images/b6yBY91Pg.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = false
            },
            new Pet
            {
                Id = 5,
                Name = "Buddy",
                Species = "Dog",
                Breed = "Beagle",
                Age = 4,
                Notes = "Sweet-natured and food-motivated. Great for training and outdoor adventures.",
                ImagePath = "https://images.dog.ceo/breeds/beagle/puppy-1.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = false
            },
            new Pet
            {
                Id = 6,
                Name = "Mittens",
                Species = "Cat",
                Breed = "Persian",
                Age = 3,
                Notes = "Elegant and serene. Prefers a quiet environment with lots of petting.",
                ImagePath = "https://cdn2.thecatapi.com/images/c0f_dBlPH.jpg",
                IsAdopted = false,
                IsPetOfTheWeek = false
            }
        );
    }
}
