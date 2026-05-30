using Microsoft.EntityFrameworkCore;
using ShelterSync.Data;
using ShelterSync.Models;

namespace ShelterSync.Services;

/// <summary>
/// Service class for managing pet data and operations.
/// Provides CRUD operations for pets using Entity Framework Core.
/// Data is persisted in PostgreSQL via ShelterSyncDbContext.
/// </summary>
public class PetService
{
    private readonly ShelterSyncDbContext _context;

    /// <summary>
    /// Initializes a new instance of the PetService.
    /// </summary>
    /// <param name="context">The database context for data operations</param>
    public PetService(ShelterSyncDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Retrieves all available pets (not adopted).
    /// </summary>
    /// <param name="species">Optional filter by species (e.g., "Dog", "Cat")</param>
    /// <returns>List of available pets matching the criteria</returns>
    public async Task<List<Pet>> GetAvailablePetsAsync(string? species = null)
    {
        var query = _context.Pets.Where(p => !p.IsAdopted);

        if (!string.IsNullOrEmpty(species))
        {
            query = query.Where(p => p.Species == species);
        }

        return await query.ToListAsync();
    }

    /// <summary>
    /// Retrieves the pet designated as Pet of the Week.
    /// </summary>
    /// <returns>Pet marked as Pet of the Week, or null if none exists</returns>
    public async Task<Pet?> GetPetOfTheWeekAsync()
    {
        return await _context.Pets.FirstOrDefaultAsync(p => p.IsPetOfTheWeek);
    }

    /// <summary>
    /// Retrieves a specific pet by ID.
    /// </summary>
    /// <param name="id">The ID of the pet to retrieve</param>
    /// <returns>The pet with the specified ID, or null if not found</returns>
    public async Task<Pet?> GetPetByIdAsync(int id)
    {
        return await _context.Pets.FindAsync(id);
    }

    /// <summary>
    /// Retrieves all pets in the system (including adopted).
    /// </summary>
    /// <returns>List of all pets</returns>
    public async Task<List<Pet>> GetAllPetsAsync()
    {
        return await _context.Pets.ToListAsync();
    }

    /// <summary>
    /// Adds a new pet to the system.
    /// </summary>
    /// <param name="pet">The pet to add</param>
    public async Task AddPetAsync(Pet pet)
    {
        _context.Pets.Add(pet);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates an existing pet's information.
    /// </summary>
    /// <param name="updatedPet">The pet with updated information</param>
    public async Task UpdatePetAsync(Pet updatedPet)
    {
        _context.Pets.Update(updatedPet);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes a pet from the system by ID.
    /// </summary>
    /// <param name="id">The ID of the pet to delete</param>
    /// <returns>True if the pet was found and deleted, false otherwise</returns>
    public async Task<bool> DeletePetAsync(int id)
    {
        var pet = await _context.Pets.FindAsync(id);
        if (pet == null)
        {
            return false;
        }

        _context.Pets.Remove(pet);
        await _context.SaveChangesAsync();
        return true;
    }
}
