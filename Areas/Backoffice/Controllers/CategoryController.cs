using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.Entities;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class CategoryController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        var categories = _unit.CategoryRepository.Get().ToList();
        return View(categories);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Category category)
    {
        _unit.CategoryRepository.Insert(category);
        _unit.Save();
        return RedirectToAction("Index");
    }

    [HttpDelete]
    public IActionResult Delete(int id)
    {
        _unit.CategoryRepository.Delete(id);
        _unit.Save();
        return RedirectToAction("Index");
    }
}