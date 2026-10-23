using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Interfaces;
using SchoolHub.Common.Enums;
using SchoolHub.Models.ViewModels.Section;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class SectionController(ICatalogService catalogService, ISectionService sectionService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await sectionService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterSectionViewModel
            {
                Grades = await catalogService.GetGradesSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterSectionViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateGradesAsync(model);
                return View(model);
            }

            var result = await sectionService.CreateAsync(model.Name, model.GradeId!.Value);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                await PopulateGradesAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await sectionService.GetForEditAsync(id); if (model is null) return NotFound(); await PopulateGradesAsync(model); return View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterSectionViewModel model)
        { if (!ModelState.IsValid) { await PopulateGradesAsync(model); return View(model); } var result = await sectionService.UpdateAsync(id, model.Name, model.GradeId!.Value); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); await PopulateGradesAsync(model); return View(model); } return RedirectToAction(nameof(Index)); }

        private async Task PopulateGradesAsync(RegisterSectionViewModel model)
        {
            model.Grades = await catalogService.GetGradesSelectListAsync();
        }
    }
}
