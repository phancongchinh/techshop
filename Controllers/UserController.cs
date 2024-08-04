using Microsoft.AspNetCore.Mvc;

namespace Techshop.Controllers;

public class UserController : Controller
{
    public IActionResult Info()
    {
        return View();
    }
}