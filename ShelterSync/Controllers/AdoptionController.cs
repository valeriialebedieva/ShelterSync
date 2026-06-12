using Microsoft.AspNetCore.Mvc;
using ShelterSync.Models;
using ShelterSync.Services;

namespace ShelterSync.Controllers;

public class AdoptionController : Controller
{
    private readonly AdoptionService _adoptionService;
    private readonly PetService _petService;

    public AdoptionController(AdoptionService adoptionService, PetService petService)
    {
        _adoptionService = adoptionService;
        _petService = petService;
    }

    [HttpGet]
    public async Task<IActionResult> Submit(int petId)
    {
        var pet = await _petService.GetPetByIdAsync(petId);
        if (pet == null) return NotFound();
        var model = new AdoptionRequest { PetId = petId };
        ViewBag.Pet = pet;
        return View("Request", model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Submit(AdoptionRequest request)
    {
        if (!ModelState.IsValid)
        {
            var pet = await _petService.GetPetByIdAsync(request.PetId);
            ViewBag.Pet = pet;
            return View("Request", request);
        }

        _adoptionService.Add(request);
        TempData["Success"] = "Your adoption request has been submitted. We'll contact you soon.";
        return RedirectToAction("Index", "Pet");
    }
}
