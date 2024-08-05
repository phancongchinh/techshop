using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.ViewModels;
using Techshop.Repository;

namespace Techshop.Controllers;

public class UserController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    [Authorize]
    public IActionResult Info()
    {
        var user = _unit.UserRepository.Get(e => e.Username == User.Identity.Name).FirstOrDefault();
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

    [HttpPost]
    [Authorize]
    public IActionResult Info(UserUpdateVm model)
    {
        if (!ModelState.IsValid) return View(model);

        var user = _unit.UserRepository.Get(x => x.Username == User.Identity.Name).FirstOrDefault();
        if (user == null) return NotFound();

        user.FullName = model.FullName;
        user.Phone = model.Phone;
        user.Email = model.Email;
        user.Address = model.Address;

        _unit.UserRepository.Update(user);
        _unit.Save();

        return View(model);
    }
}