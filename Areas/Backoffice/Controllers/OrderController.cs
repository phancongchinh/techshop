using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class OrderController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult OrderList()
    {
        var orders = _unit.OrderRepository.Get().ToList();
        return View(orders);
    }
}