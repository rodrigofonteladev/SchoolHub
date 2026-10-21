using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Interfaces;
using SchoolHub.Common.Enums;
using SchoolHub.Models.ViewModels.Level;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class LevelController(ILevelService levelService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await levelService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterLevelViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await levelService.CreateAsync(model.Name);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
