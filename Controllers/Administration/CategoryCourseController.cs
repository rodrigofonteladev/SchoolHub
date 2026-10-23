using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.CategoryCourse;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CategoryCourseController(ICategoryCourseService categoryCourseService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index() => View(await categoryCourseService.GetAllAsync());

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterCategoryCourseViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await categoryCourseService.CreateAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await categoryCourseService.GetForEditAsync(id); return model is null ? NotFound() : View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterCategoryCourseViewModel model) { if (!ModelState.IsValid) return View(model); var result = await categoryCourseService.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(model); } return RedirectToAction(nameof(Index)); }
    }
}
