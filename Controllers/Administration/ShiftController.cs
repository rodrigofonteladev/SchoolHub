using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Shift;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class ShiftController(IShiftService service) : Controller
    {
        public async Task<IActionResult> Index() => View(await service.GetAllAsync());
        [HttpGet] public IActionResult Create() => View();
        [HttpPost] public async Task<IActionResult> Create(RegisterShiftViewModel model)
        {
            if (!ModelState.IsValid) return View(model);
            var result = await service.CreateAsync(model.Name);
            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(model); }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await service.GetForEditAsync(id); return model is null ? NotFound() : View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterShiftViewModel model) { if (!ModelState.IsValid) return View(model); var result = await service.UpdateAsync(id, model.Name); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(model); } return RedirectToAction(nameof(Index)); }
    }
}
