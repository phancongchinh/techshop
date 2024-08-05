using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Controllers;

public class ProductController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    [Route("Product/{id:int}")]
    public IActionResult Info(int? id)
    {
        if (id == null) return RedirectToAction("Index", "Home");

        var product = _unit.ProductRepository.Get(e => e.Id == id, includeProperties: "Images,Categories")
            .FirstOrDefault();

        if (product == null) return NotFound();

        return View(product);
    }

    public IActionResult Search(string q)
    {
        var products = _unit.ProductRepository.Get(e => e.Name.Contains(q), includeProperties: "Images").ToList();
        return View(products);
    }
}