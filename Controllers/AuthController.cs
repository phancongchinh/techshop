using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Techshop.Models.Entities;
using Techshop.Models.ViewModels;
using Techshop.Repository;
using Techshop.Services;

namespace Techshop.Controllers;

public class AuthController : Controller
{
    private readonly UnitOfWork _unit = new();

    public IActionResult Login()
    {
        return View();
    }

    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task Authenticate(User user)
    {
        var claims = new List<Claim>
        {
            new(ClaimsIdentity.DefaultNameClaimType, user.Username),
            new(ClaimsIdentity.DefaultRoleClaimType, user.Role.Name)
        };
        ClaimsIdentity id = new(
            claims, "ApplicationCookie",
            ClaimsIdentity.DefaultNameClaimType,
            ClaimsIdentity.DefaultRoleClaimType
        );
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
    }

    [HttpPost]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid) return View(model);

        if (model.Password != model.RetypePassword)
        {
            ModelState.AddModelError("", "Nhập lại mật khẩu không chính xác!");
            return View(model);
        }


        var user = _unit.UserRepository.Get(x => x.Username == model.Username).FirstOrDefault();
        if (user == null)
        {
            user = new User
            {
                FullName = model.FullName,
                Phone = model.Phone,
                Email = model.Email,
                Password = PasswordConverter.Hash(model.Password),
            };
            var userRole = _unit.RoleRepository.Get(x => x.Id == 2).FirstOrDefault();
            if (userRole != null) user.Role = userRole;

            _unit.UserRepository.Insert(user);
            _unit.Save();

            await Authenticate(user);
            return RedirectToAction("Index", "Home");
        }

        ModelState.AddModelError("", "The same account already exist");
        return View(model);
    }
}