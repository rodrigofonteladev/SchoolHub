using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.DayWeek;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class DayWeekController(IDayWeekService dayWeekService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index() => View(await dayWeekService.GetAllAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDayWeekViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await dayWeekService.CreateAsync(model.Name);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await dayWeekService.GetForEditAsync(id);
            return model is null ? NotFound() : View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, RegisterDayWeekViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await dayWeekService.UpdateAsync(id, model.Name);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
