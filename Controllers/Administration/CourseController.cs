using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Course;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class CourseController(ICourseService courseService, ICatalogService catalogService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index() => View(await courseService.GetAllAsync());

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterCourseViewModel
            {
                Categories = await catalogService.GetCategoryCoursesSelectListAsync()
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterCourseViewModel model)
        {
            if (!ModelState.IsValid)
            {
                await PopulateCategoriesAsync(model);
                return View(model);
            }

            var result = await courseService.CreateAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                await PopulateCategoriesAsync(model);
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await courseService.GetForEditAsync(id); if (model is null) return NotFound(); await PopulateCategoriesAsync(model); return View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterCourseViewModel model) { if (!ModelState.IsValid) { await PopulateCategoriesAsync(model); return View(model); } var result = await courseService.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); await PopulateCategoriesAsync(model); return View(model); } return RedirectToAction(nameof(Index)); }

        private async Task PopulateCategoriesAsync(RegisterCourseViewModel model)
        {
            model.Categories = await catalogService.GetCategoryCoursesSelectListAsync();
        }
    }
}
