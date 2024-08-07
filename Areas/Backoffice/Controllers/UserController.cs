using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.ViewModels;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class UserController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        var users = _unit.UserRepository.Get(includeProperties: "Role").ToList();
        return View(users);
    }

    public IActionResult Info(int id)
    {
        var user = _unit.UserRepository.Get(e => e.Id == id).FirstOrDefault();
        if (user == null) return NotFound();

        var model = new UserUpdateVm()
        {
            FullName = user.FullName,
            Phone = user.Phone,
            Email = user.Email,
            Address = user.Address
        };

        return View(model);
    }
}