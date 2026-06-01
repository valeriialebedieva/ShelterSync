using Microsoft.AspNetCore.Mvc;
using ShelterSync.Models;
using ShelterSync.Services;

namespace ShelterSync.Controllers;

/// <summary>
/// Controller for managing pet-related operations and views.
/// Handles CRUD operations for pets and pet search functionality.
/// </summary>
public class PetController : Controller
{
    private readonly PetService _petService;

    /// <summary>
    /// Maximum allowed file size for pet photo uploads (5MB).
    /// </summary>
    private const long MaxFileSize = 5 * 1024 * 1024;

    /// <summary>
    /// Initializes a new instance of the PetController.
    /// </summary>
    /// <param name="petService">The pet service for data operations</param>
    public PetController(PetService petService)
    {
        _petService = petService;
    }

    /// <summary>
    /// Displays a list of all pets with optional search filtering.
    /// </summary>
    /// <param name="searchString">Optional search term to filter pets by name</param>
    /// <returns>View with list of pets</returns>
    public async Task<IActionResult> Index(string? searchString)
    {
        var pets = await _petService.GetAvailablePetsAsync();

        if (!string.IsNullOrEmpty(searchString))
        {
            pets = pets
                .Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        return View(pets);
    }

    /// <summary>
    /// Displays the create pet form.
    /// </summary>
    /// <returns>Create view</returns>
    [HttpGet]
    public IActionResult Create() => View();

    /// <summary>
    /// Handles the creation of a new pet with optional image upload.
    /// </summary>
    /// <param name="newPet">The pet data from the form</param>
    /// <param name="petPhoto">Optional image file for the pet</param>
    /// <returns>Redirects to Index on success, returns Create view on error</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(Pet newPet, IFormFile? petPhoto)
    {
        if (petPhoto == null || petPhoto.Length == 0)
        {
            ModelState.AddModelError("petPhoto", "Pet photo is required");
        }

        if (!ModelState.IsValid)
        {
            return View(newPet);
        }

        try
        {
            if (petPhoto != null)
            {
                var imagePath = await SavePetPhotoAsync(petPhoto);
                if (imagePath == null)
                {
                    ModelState.AddModelError("petPhoto", "File size cannot exceed 5MB");
                    return View(newPet);
                }

                newPet.ImagePath = imagePath;
            }

            await _petService.AddPetAsync(newPet);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return View(newPet);
        }
    }

    /// <summary>
    /// Displays the edit form for a specific pet.
    /// </summary>
    /// <param name="id">The ID of the pet to edit</param>
    /// <returns>Edit view with pet data, or NotFound if pet doesn't exist</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var pet = await _petService.GetPetByIdAsync(id);
        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    /// <summary>
    /// Handles the update of an existing pet.
    /// </summary>
    /// <param name="id">The ID of the pet to update</param>
    /// <param name="pet">The updated pet data</param>
    /// <param name="petPhoto">Optional new image file</param>
    /// <returns>Redirects to Index on success, returns Edit view on error</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, Pet pet, IFormFile? petPhoto)
    {
        if (id != pet.Id)
        {
            return BadRequest();
        }

        if (!ModelState.IsValid)
        {
            return View(pet);
        }

        try
        {
            var existingPet = await _petService.GetPetByIdAsync(id);
            if (existingPet == null)
            {
                return NotFound();
            }

            if (petPhoto != null && petPhoto.Length > 0)
            {
                var imagePath = await SavePetPhotoAsync(petPhoto);
                if (imagePath == null)
                {
                    ModelState.AddModelError("petPhoto", "File size cannot exceed 5MB");
                    return View(pet);
                }

                pet.ImagePath = imagePath;
            }
            else
            {
                // Keep existing image if no new image provided
                pet.ImagePath = existingPet.ImagePath;
            }

            await _petService.UpdatePetAsync(pet);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, $"An error occurred: {ex.Message}");
            return View(pet);
        }
    }

    /// <summary>
    /// Displays delete confirmation for a pet.
    /// </summary>
    /// <param name="id">The ID of the pet to delete</param>
    /// <returns>Delete confirmation view, or NotFound if pet doesn't exist</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var pet = await _petService.GetPetByIdAsync(id);
        if (pet == null)
        {
            return NotFound();
        }

        return View(pet);
    }

    /// <summary>
    /// Handles the deletion of a pet.
    /// </summary>
    /// <param name="id">The ID of the pet to delete</param>
    /// <returns>Redirects to Index after deletion</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        try
        {
            var pet = await _petService.GetPetByIdAsync(id);
            if (pet == null)
            {
                return NotFound();
            }

            // Delete associated image if it exists and is a local file
            if (!string.IsNullOrEmpty(pet.ImagePath) && !pet.ImagePath.StartsWith("http"))
            {
                var fullImagePath = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot",
                    pet.ImagePath.TrimStart('/'));

                if (System.IO.File.Exists(fullImagePath))
                {
                    System.IO.File.Delete(fullImagePath);
                }
            }

            await _petService.DeletePetAsync(id);
            return RedirectToAction(nameof(Index));
        }
        catch (Exception ex)
        {
            TempData["Error"] = $"An error occurred while deleting the pet: {ex.Message}";
            return RedirectToAction(nameof(Index));
        }
    }

    /// <summary>
    /// Saves an uploaded pet photo to the wwwroot/images directory.
    /// Returns the relative URL path, or null if the file exceeds the size limit.
    /// </summary>
    /// <param name="photo">The uploaded file</param>
    /// <returns>Relative image path (e.g., "/images/guid.jpg"), or null if invalid</returns>
    private async Task<string?> SavePetPhotoAsync(IFormFile photo)
    {
        if (photo.Length > MaxFileSize)
        {
            return null;
        }

        var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
        var imagesDirectory = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images");

        if (!Directory.Exists(imagesDirectory))
        {
            Directory.CreateDirectory(imagesDirectory);
        }

        var savePath = Path.Combine(imagesDirectory, fileName);

        await using var stream = new FileStream(savePath, FileMode.Create);
        await photo.CopyToAsync(stream);

        return "/images/" + fileName;
    }
}
