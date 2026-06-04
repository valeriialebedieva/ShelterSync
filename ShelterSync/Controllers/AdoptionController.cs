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
    public IActionResult Request(int petId)
    {
        var pet = _petService.GetPetById(petId);
        if (pet == null) return NotFound();
        var model = new AdoptionRequest { PetId = petId };
        ViewBag.Pet = pet;
        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Request(AdoptionRequest request)
    {
        if (!ModelState.IsValid)
        {
            var pet = _petService.GetPetById(request.PetId);
            ViewBag.Pet = pet;
            return View(request);
        }

        _adoptionService.Add(request);
        TempData["Success"] = "Your adoption request has been submitted. We'll contact you soon.";
        return RedirectToAction("Index", "Pet");
    }
}