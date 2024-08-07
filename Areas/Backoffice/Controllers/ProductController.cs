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
    private readonly IWebHostEnvironment _hostEnvironment;

    public ProductController(IWebHostEnvironment webHostEnvironment)
    {
        _hostEnvironment = webHostEnvironment;
    }


    public IActionResult Index()
    {
        var products = _unit.ProductRepository.Get(includeProperties: "Images").ToList();
        return View(products);
    }

    public IActionResult Create()
    {
        var categories = _unit.CategoryRepository.Get().ToList();
        var productVm = new ProductVm()
        {
            Categories = categories
        };

        return View(productVm);
    }

    [HttpPost]
    public IActionResult Create(ProductVm productVm)
    {
        if (!ModelState.IsValid) return View(productVm);


        var categories = _unit.CategoryRepository.Get(e => productVm.SelectedCategoryIds.Contains(e.Id)).ToList();

        var product = new Product()
        {
            Brand = productVm.Brand,
            Model = productVm.Model,
            Price = productVm.Price,
            Description = productVm.Description,
            Quantity = productVm.Quantity,
            Categories = categories
        };

        _unit.ProductRepository.Insert(product);
        _unit.Save();

        var files = HttpContext.Request.Form.Files;

        if (files.Count > 0)
        {
            foreach (var item in files)
            {
                var rootPath = _hostEnvironment.WebRootPath;
                var uploads = Path.Combine(rootPath, "uploads/images");
                var extension = Path.GetExtension(item.FileName);
                var dynamicFileName = Guid.NewGuid() + "_" + product.Id + extension;
                using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                {
                    item.CopyTo(filesStream);
                }

                //add product Image for new product

                var image = new Image { Path = dynamicFileName, ProductId = product.Id };

                _unit.ImageRepository.Insert(image);
            }
        }

        _unit.Save();

        return RedirectToAction("Index", "Product");
    }

    [HttpGet]
    public IActionResult Update(int id)
    {
        var product = _unit.ProductRepository.Get(x => x.Id == id, includeProperties: "Images,Categories")
            .FirstOrDefault();
        if (product == null) return NotFound();

        var categories = _unit.CategoryRepository.Get().ToList();

        var productVm = new ProductVm()
        {
            Brand = product.Brand,
            Model = product.Model,
            Price = product.Price,
            Description = product.Description,
            Quantity = product.Quantity,
            Categories = categories,
            SelectedCategoryIds = product.Categories.Select(c => c.Id).ToArray()
        };

        return View(productVm);
    }

    [HttpPost]
    public IActionResult Update(int id, ProductVm productVm)
    {
        // if (!ModelState.IsValid) return View(productVm);
        var product = _unit.ProductRepository.Get(x => x.Id == id, includeProperties: "Categories").FirstOrDefault();
        if (product == null) return NotFound();

        var categories = _unit.CategoryRepository.Get(e => productVm.SelectedCategoryIds.Contains(e.Id)).ToList();

        product.Brand = productVm.Brand;
        product.Model = productVm.Model;
        product.Price = productVm.Price;
        product.Description = productVm.Description;
        product.Quantity = productVm.Quantity;
        product.Categories = categories;

        _unit.ProductRepository.Update(product);
        _unit.Save();

        var files = HttpContext.Request.Form.Files;

        if (files.Count > 0)
        {
            var oldImages = _unit.ImageRepository.Get(e => e.ProductId == product.Id).ToList();
            foreach (var image in oldImages)
            {
                _unit.ImageRepository.Delete(image);
            }

            _unit.Save();

            foreach (var item in files)
            {
                var rootPath = _hostEnvironment.WebRootPath;
                var uploads = Path.Combine(rootPath, "uploads/images");
                var extension = Path.GetExtension(item.FileName);
                var dynamicFileName = Guid.NewGuid() + "_" + product.Id + extension;
                using (var filesStream = new FileStream(Path.Combine(uploads, dynamicFileName), FileMode.Create))
                {
                    item.CopyTo(filesStream);
                }

                //add product Image for new product

                var image = new Image { Path = dynamicFileName, ProductId = product.Id };

                _unit.ImageRepository.Insert(image);
            }
        }

        return RedirectToAction("Index", "Product");
    }
}