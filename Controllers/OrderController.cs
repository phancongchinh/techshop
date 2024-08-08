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
    [Route("/Order/{id:int}")]
    public IActionResult Info(int id)
    {
        var username = User.Identity.Name;
        var user = _unit.UserRepository.Get(e => e.Username == username).FirstOrDefault();
        if (user == null)
            return NotFound();

        var order = _unit.OrderRepository.Get(e => e.Id == id && e.UserId == user.Id, includeProperties: "OrderItems")
            .FirstOrDefault();

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

    [Authorize]
    [HttpPost]
    public IActionResult CreateOrder()
    {
        const string quantityInvalid = "Can not make purchase! Quantity is invalid!";

        var user = _unit.UserRepository.Get(x => x.Username == User.Identity.Name).First();

        IEnumerable<CartItem> cartItems = _unit.CartItemRepository
            .Get(x => x.User.Username == User.Identity.Name, includeProperties: "Product").ToList();

        if (cartItems.Any(item => item.Quantity > item.Product.Quantity))
        {
            TempData["Message"] = quantityInvalid;
            return RedirectToAction("Index", "CartItem");
        }

        Order newOrder = new()
        {
            UserId = user.Id,
            Status = OrderStatus.Pending,
            CreatedAt = DateTime.Now,
            CompletedAt = null
        };

        _unit.OrderRepository.Insert(newOrder);
        _unit.Save();

        foreach (var item in cartItems)
        {
            _unit.OrderItemRepository.Insert(new OrderItem()
            {
                OrderId = newOrder.Id,
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

        return RedirectToAction("Info", "Order", newOrder.Id);
    }
}