using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Tutor;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class TutorController(ITutorService service, ICatalogService catalog) : Controller
    {
        public async Task<IActionResult> Index() => View(await service.GetAllAsync());
        [HttpGet] public async Task<IActionResult> Create() => View(await PopulateAsync(new RegisterTutorViewModel()));
        [HttpPost] public async Task<IActionResult> Create(RegisterTutorViewModel model)
        {
            if (!ModelState.IsValid) return View(await PopulateAsync(model));
            var result = await service.CreateAsync(model);
            if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); }
            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await service.GetForEditAsync(id); if (model is null) return NotFound(); return View(await PopulateAsync(model)); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterTutorViewModel model) { if (!ModelState.IsValid) return View(await PopulateAsync(model)); var result = await service.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(await PopulateAsync(model)); } return RedirectToAction(nameof(Index)); }
        private async Task<RegisterTutorViewModel> PopulateAsync(RegisterTutorViewModel model) { model.DocumentTypes = await catalog.GetDocumentTypesSelectListAsync(); model.Students = await catalog.GetStudentsSelectListAsync(); return model; }
    }
}
