using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.Entities;
using Techshop.Models.enums;
using Techshop.Repository;

namespace Techshop.Controllers;

public class OrderController : Controller
{
    private readonly UnitOfWork _unit = new();

    [Authorize]
    public IActionResult Index()
    {
        // var user = _unit.UserRepository.Get(x => x.Username == User.Identity.Name).First();

        IEnumerable<Order> orders = _unit.OrderRepository
            .Get(e => e.User.Username == User.Identity.Name, includeProperties: "User,OrderItems")
            .OrderByDescending(x => x.CreatedAt);

        return View(orders);
    }

    [Authorize]
    public IActionResult Info(int orderId)
    {
        var order = _unit.OrderRepository.GetById(orderId);

        if (order == null) return NotFound();

        return View(order);
    }

    [Authorize]
    [HttpPost]
    public IActionResult CreateOrder()
    {
        var user = _unit.UserRepository.Get(x => x.Username == User.Identity.Name).First();

        IEnumerable<CartItem> cartItems = _unit.CartItemRepository
            .Get(x => x.User.Username == User.Identity.Name, includeProperties: "Product").ToList();

        if (cartItems.Any(item => item.Quantity > item.Product.Quantity))
        {
            TempData["Message"] = "Can not make purchase! Quantity is invalid!";
            return RedirectToAction("Index", "CartItem");
        }

        Order newPurchase = new()
        {
            UserId = user.Id,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.Now,
        };

        _unit.OrderRepository.Insert(newPurchase);
        _unit.Save();

        foreach (var item in cartItems)
        {
            _unit.OrderItemRepository.Insert(new OrderItem()
            {
                OrderId = newPurchase.Id,
                ProductId = item.ProductId,
                Price = item.Product.Price,
                Quantity = item.Quantity
            });
            _unit.CartItemRepository.Delete(item);

            var product = _unit.ProductRepository.GetById(item.ProductId)!;
            product.Quantity -= item.Quantity;
            _unit.ProductRepository.Update(product);
        }

        _unit.Save();

        return RedirectToAction("Info", "Order", new { purchaseId = newPurchase.Id });
    }
}