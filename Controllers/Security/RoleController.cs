using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Interfaces;
using SchoolHub.Common.Enums;
using SchoolHub.Models.ViewModels.Role;

namespace SchoolHub.Controllers.Security
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class RoleController(IRoleService roleService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await roleService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterRoleViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await roleService.CreateRoleAsync(model.RoleName);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
