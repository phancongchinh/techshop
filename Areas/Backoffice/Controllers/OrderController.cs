using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class OrderController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        var orders = _unit.OrderRepository.Get(includeProperties: "User,OrderItems").ToList();
        return View(orders);
    }

    [Authorize]
    [Route("/Backoffice/Order/{id:int}")]
    public IActionResult Info(int id)
    {
        var order = _unit.OrderRepository.Get(e => e.Id == id, includeProperties: "OrderItems").FirstOrDefault();

        if (order == null) return NotFound();
        
        var orderItems = order.OrderItems;

        foreach (var item in orderItems)
        {
            var product = _unit.ProductRepository.Get(p => p.Id == item.ProductId, includeProperties: "Images")
                .FirstOrDefault();
            if (product == null) continue;
            item.Product = product;
        }

        return View(order);
    }
}