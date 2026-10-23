using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Enrollment;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class EnrollmentController(IEnrollmentService service, ICatalogService catalog) : Controller
    {
        public async Task<IActionResult> Index() => View(await service.GetAllAsync());
        [HttpGet] public async Task<IActionResult> Create() => View(await PopulateAsync(new RegisterEnrollmentViewModel()));
        [HttpPost] public async Task<IActionResult> Create(RegisterEnrollmentViewModel model)
        {
            if (!ModelState.IsValid) return View(await PopulateAsync(model));
            var result = await service.CreateAsync(model);
            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await service.GetForEditAsync(id); if (model is null) return NotFound(); return View(await PopulateAsync(model)); }
        [HttpPost] public async Task<IActionResult> Edit(int id, EditEnrollmentViewModel model) { if (!ModelState.IsValid) return View(await PopulateAsync(model)); var result = await service.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); } return RedirectToAction(nameof(Index)); }
        private async Task<RegisterEnrollmentViewModel> PopulateAsync(RegisterEnrollmentViewModel model) { model.Students = await catalog.GetStudentsSelectListAsync(); model.Sections = await catalog.GetSectionsSelectListAsync(); model.Reasons = await catalog.GetReasonEnrollmentsSelectListAsync(); return model; }
        private async Task<EditEnrollmentViewModel> PopulateAsync(EditEnrollmentViewModel model) { model.Sections = await catalog.GetSectionsSelectListAsync(); model.Reasons = await catalog.GetReasonEnrollmentsSelectListAsync(); return model; }
    }
}
