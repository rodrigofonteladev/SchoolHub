using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.ClassAsignment;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class ClassAsignmentController(IClassAsignmentService service, ICatalogService catalog) : Controller
    {
        public async Task<IActionResult> Index() => View(await service.GetAllAsync());
        [HttpGet] public async Task<IActionResult> Create() => View(await PopulateAsync(new RegisterClassAsignmentViewModel()));
        [HttpPost] public async Task<IActionResult> Create(RegisterClassAsignmentViewModel model)
        {
            if (!ModelState.IsValid) return View(await PopulateAsync(model));
            var result = await service.CreateAsync(model);
            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await service.GetForEditAsync(id); if (model is null) return NotFound(); return View(await PopulateAsync(model)); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterClassAsignmentViewModel model) { if (!ModelState.IsValid) return View(await PopulateAsync(model)); var result = await service.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); } return RedirectToAction(nameof(Index)); }
        private async Task<RegisterClassAsignmentViewModel> PopulateAsync(RegisterClassAsignmentViewModel model) { model.Teachers = await catalog.GetTeachersSelectListAsync(); model.Courses = await catalog.GetCoursesSelectListAsync(); model.Schedules = await catalog.GetSchedulesSelectListAsync(); model.Sections = await catalog.GetSectionsSelectListAsync(); return model; }
    }
}
