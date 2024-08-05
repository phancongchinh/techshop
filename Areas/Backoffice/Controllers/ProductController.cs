using System.Net;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Techshop.Models.Entities;
using Techshop.Models.ViewModels;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class ProductController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        var products = _unit.ProductRepository.Get().ToList();
        return View(products);
    }

    public IActionResult AddProduct()
    {
        var categories = _unit.CategoryRepository.Get().ToList();
        ViewBag.Categories = new SelectList(categories, "Id", "Name");
        return View();
    }

    [HttpPost]
    public IActionResult AddProduct(ProductVm productVm)
    {
        if (!ModelState.IsValid) return View(productVm);

        // foreach (var file in productVm.Images)
        // {
        //     string filePath = 
        // }
        
        var product = new Product()
        {
            Brand = productVm.Brand,
            Model = productVm.Model,
            Price = productVm.Price,
            Description = productVm.Description,
            Quantity = productVm.Quantity,
        };

        _unit.ProductRepository.Insert(product);
        _unit.Save();
        return View("Index");
    }

    [HttpGet]
    public IActionResult ProductInfo(int id)
    {
        var product = _unit.ProductRepository.Get(x => x.Id == id, includeProperties: "Images,Categories")
            .FirstOrDefault();
        if (product == null) return NotFound();

        var productVm = new ProductVm()
        {
            Brand = product.Brand,
            Model = product.Model,
            Price = product.Price,
            Description = product.Description,
            Quantity = product.Quantity,
        };

        return View(productVm);
    }

    [HttpPost]
    public IActionResult ProductInfo(int id, ProductVm productVm)
    {
        if (!ModelState.IsValid) return View(productVm);
        var product = _unit.ProductRepository.Get(x => x.Id == id, includeProperties: "Images,Categories")
            .FirstOrDefault();
        if (product == null) return NotFound();

        product.Brand = productVm.Brand;
        product.Model = productVm.Model;
        product.Price = productVm.Price;
        product.Description = productVm.Description;
        product.Quantity = productVm.Quantity;

        _unit.ProductRepository.Update(product);
        _unit.Save();

        return View();
    }
}