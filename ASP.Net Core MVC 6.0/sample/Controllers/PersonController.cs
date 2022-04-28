using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using sample.Models;

namespace sample.Controllers;

public class PersonController : Controller
{
    private readonly ILogger<PersonController> _logger;

    public PersonController(ILogger<PersonController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {   
        var people = new List<PersonModel>()
        {
            new PersonModel {Id = 1, FirstName = "John", LastName = "Fuck", Gender = "Male", Address="Kabukicho"},
            new PersonModel {Id = 2, FirstName = "John", LastName = "Wich", Gender = "Male", Address="Kabukicho"},
            new PersonModel {Id = 3, FirstName = "John", LastName = "ASS", Gender = "Female", Address="Kabukicho"}
        };
        return View(people);
    }
    public IActionResult Add()
    {   
        return View();
    }

    [HttpPost]
    public IActionResult Add(PersonModel person)
    {
        return View($"This is a new person {person.FirstName} {person.LastName}");
    }
  
}
