using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models;
using Techshop.Models.ViewModels;
using Techshop.Repository;

namespace Techshop.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly UnitOfWork _unit = new UnitOfWork();

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        var products = _unit.ProductRepository.Get(includeProperties: "Images").ToList();
        var categories = _unit.CategoryRepository.Get().ToList();

        var homeVm = new HomeVm
        {
            Products = products,
            Categories = categories
        };

        return View(homeVm);
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}