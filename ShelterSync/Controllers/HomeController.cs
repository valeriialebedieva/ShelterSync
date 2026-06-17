using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using ShelterSync.Models;
using ShelterSync.Services;

namespace ShelterSync.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly PetService _petService;

    public HomeController(ILogger<HomeController> logger, PetService petService)
    {
        _logger = logger;
        _petService = petService;
    }

    public async Task<IActionResult> Index(string? searchString, string? species)
    {
        var pets = await _petService.GetAvailablePets(species);

        if (!string.IsNullOrEmpty(searchString))
        {
            pets = pets
                .Where(p => p.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                .ToList();
        }

        ViewBag.PetOfTheWeek = await _petService.GetPetOfTheWeek();
        ViewBag.SelectedSpecies = species;
        ViewBag.AvailableSpecies = await _petService.GetAvailableSpecies();
        ViewBag.CurrentFilter = searchString;

        return View(pets);
    }

    public async Task<IActionResult> WhyAdopt()
    {
        var pets = await _petService.GetAvailablePets();
        return View(pets);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    public IActionResult Contact()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
