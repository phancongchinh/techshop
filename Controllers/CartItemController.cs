using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.Entities;
using Techshop.Repository;

namespace Techshop.Controllers;

public class CartItemController : Controller
{
    private readonly UnitOfWork _unit = new();

    [Authorize]
    public IActionResult Index()
    {
        IEnumerable<CartItem> cartItems = _unit.CartItemRepository
            .Get(x => x.User.Username == User.Identity.Name, includeProperties: "User,Product")
            .ToList();

        return View(cartItems);
    }

    [Authorize]
    [HttpPost]
    public IActionResult UpdateCart(int productId, int quantity, bool forceUpdateQuantity)
    {
        var product = _unit.ProductRepository.GetById(productId);

        if (product == null) return NotFound();

        if (quantity > product.Quantity)
        {
            TempData["Message"] = "You can not order that much!";
            return RedirectToAction("Index", "CartItem");
        }

        var item = _unit.CartItemRepository
            .Get(x => x.User.Username == User.Identity.Name && x.ProductId == productId,
                includeProperties: "User").FirstOrDefault();

        var user = _unit.UserRepository.Get(x => x.Username == User.Identity.Name).First();
        if (item == null)
        {
            _unit.CartItemRepository.Insert(new CartItem()
            {
                UserId = user.Id,
                ProductId = productId,
                Quantity = quantity
            });
        }
        else
        {
            item.Quantity = forceUpdateQuantity ? quantity : item.Quantity + quantity;
            _unit.CartItemRepository.Update(item);
        }

        _unit.Save();

        return RedirectToAction("Index", "CartItem");
    }

    [Authorize]
    [HttpPost]
    public IActionResult RemoveItem(int productId)
    {
        var cartItem = _unit.CartItemRepository
            .Get(x => x.User.Username == User.Identity.Name && x.ProductId == productId,
                includeProperties: "User").FirstOrDefault();

        if (cartItem == null)
        {
            TempData["Message"] = "Item not valid!";
            return RedirectToAction("Index", "CartItem");
        }

        _unit.CartItemRepository.Delete(cartItem);
        _unit.Save();

        return RedirectToAction("Index", "CartItem");
    }
}