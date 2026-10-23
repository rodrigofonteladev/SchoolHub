using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Grade;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class GradeController(ICatalogService catalogService, IGradeService gradeService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await gradeService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterGradeViewModel
            {
                Levels = await catalogService.GetLevelsSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterGradeViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateLevelsAsync(model);
                return View(model);
            }

            var result = await gradeService.CreateAsync(model.Name, model.LevelId!.Value);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                await PopulateLevelsAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await gradeService.GetForEditAsync(id); if (model is null) return NotFound(); await PopulateLevelsAsync(model); return View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterGradeViewModel model)
        { if (!ModelState.IsValid) { await PopulateLevelsAsync(model); return View(model); } var result = await gradeService.UpdateAsync(id, model.Name, model.LevelId!.Value); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); await PopulateLevelsAsync(model); return View(model); } return RedirectToAction(nameof(Index)); }

        private async Task PopulateLevelsAsync(RegisterGradeViewModel model)
        {
            model.Levels = await catalogService.GetLevelsSelectListAsync();
        }
    }
}
