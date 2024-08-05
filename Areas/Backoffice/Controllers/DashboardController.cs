using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class DashboardController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        return View();
    }
}