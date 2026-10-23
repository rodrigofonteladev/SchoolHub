using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.ReasonEnrollment;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class ReasonEnrollmentController(IReasonEnrollmentService reasonEnrollmentService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await reasonEnrollmentService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterReasonEnrollmentViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await reasonEnrollmentService.CreateAsync(model.Description);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await reasonEnrollmentService.GetForEditAsync(id); return model is null ? NotFound() : View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, RegisterReasonEnrollmentViewModel model)
        { if (!ModelState.IsValid) return View(model); var result = await reasonEnrollmentService.UpdateAsync(id, model.Description); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); return View(model); } return RedirectToAction(nameof(Index)); }
    }
}
