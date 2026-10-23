using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Schedule;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class ScheduleController(IScheduleService service, ICatalogService catalog) : Controller
    {
        public async Task<IActionResult> Index() => View(await service.GetAllAsync());
        [HttpGet] public async Task<IActionResult> Create() => View(await PopulateAsync(new RegisterScheduleViewModel()));

        [HttpPost]
        public async Task<IActionResult> Create(RegisterScheduleViewModel model)
        {
            if (!ModelState.IsValid) return View(await PopulateAsync(model));
            var result = await service.CreateAsync(model);
            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await service.GetForEditAsync(id); if (model is null) return NotFound(); return View(await PopulateAsync(model)); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterScheduleViewModel model) { if (!ModelState.IsValid) return View(await PopulateAsync(model)); var result = await service.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); } return RedirectToAction(nameof(Index)); }
        private async Task<RegisterScheduleViewModel> PopulateAsync(RegisterScheduleViewModel model) { model.Shifts = await catalog.GetShiftsSelectListAsync(); model.Days = await catalog.GetDaysSelectListAsync(); return model; }
    }
}
