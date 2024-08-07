using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Controllers;

public class ProductController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    [Route("/Product/{id:int}")]
    public IActionResult Info(int id)
    {
        var product = _unit.ProductRepository.Get(e => e.Id == id, includeProperties: "Images,Categories")
            .FirstOrDefault();

        if (product == null) return NotFound();

        return View(product);
    }

    [HttpGet]
    public IActionResult Search(int categoryId, string search)
    {
        search ??= "";
        var products = _unit.ProductRepository.Get(
            p => (p.Brand + p.Model).ToLower().Contains(search.ToLower()) && p.Categories.Select(c => c.Id).Contains(categoryId),
            includeProperties: "Images").ToList();
        
        return View(products);
    }
}