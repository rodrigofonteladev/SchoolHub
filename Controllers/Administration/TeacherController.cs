using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Teacher;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class TeacherController(ITeacherService teacherService, ICatalogService catalogService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await teacherService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterTeacherViewModel
            {
                DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync()
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterTeacherViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateDocumentTypesAsync(model);
                return View(model);
            }

            var result = await teacherService.CreateAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                await PopulateDocumentTypesAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await teacherService.GetForEditAsync(id); if (model is null) return NotFound(); await PopulateDocumentTypesAsync(model); return View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterTeacherViewModel model)
        { if (!ModelState.IsValid) { await PopulateDocumentTypesAsync(model); return View(model); } var result = await teacherService.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); await PopulateDocumentTypesAsync(model); return View(model); } return RedirectToAction(nameof(Index)); }

        private async Task PopulateDocumentTypesAsync(RegisterTeacherViewModel model)
        {
            model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync();
        }
    }
}
