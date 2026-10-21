using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.DocumentType;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class DocumentTypeController(IDocumentTypeService documentTypeService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await documentTypeService.GetAllAsync());
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(RegisterDocumentTypeViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var result = await documentTypeService.CreateAsync(model.Name, model.IsActive);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }
    }
}
