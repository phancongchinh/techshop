using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Techshop.Repository;

namespace Techshop.Areas.Backoffice.Controllers;

[Area("Backoffice")]
[Authorize(Roles = "Administrator")]
public class RoleController : Controller
{
    private readonly UnitOfWork _unit = new UnitOfWork();

    public IActionResult Index()
    {
        var roles = _unit.RoleRepository.Get().ToList();
        return View(roles);
    }
}