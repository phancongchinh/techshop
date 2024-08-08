using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.enums;
using Techshop.Models.ViewModels;
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

        var model = new OrderUpdateVm()
        {
            Order = order,
            Status = order.Status,
            OrderStatusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList()
        };

        return View(model);
    }

    [HttpPost]
    public IActionResult Update(int id, OrderUpdateVm vm)
    {
        var statusValues = Enum.GetValues(typeof(OrderStatus)).Cast<OrderStatus>().ToList();
        if (!statusValues.Contains(vm.Status))
            return NotFound();

        var order = _unit.OrderRepository.Get(e => e.Id == id, includeProperties: "OrderItems").FirstOrDefault();
        if (order == null) return NotFound();

        if (order.Status == vm.Status || order.Status == OrderStatus.Cancelled)
            return Redirect("/Backoffice/Order/" + id);

        order.Status = vm.Status;
        if (OrderStatus.Completed == vm.Status)
        {
            order.CompletedAt = DateTime.Now;
        }
        else if (OrderStatus.Cancelled == vm.Status)
        {
        }

        _unit.OrderRepository.Update(order);
        _unit.Save();

        return Redirect("/Backoffice/Order/" + id);
    }
}