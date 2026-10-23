using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;
using SchoolHub.Models.ViewModels.Student;
using SchoolHub.Models.ViewModels.Tutor;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class StudentController(
        IStudentService studentService,
        ICatalogService catalogService) : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await studentService.GetAllAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new RegisterStudentViewModel
            {
                DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync(),
                Sections = await catalogService.GetSectionsSelectListAsync(),
                ReasonEnrollments = await catalogService.GetReasonEnrollmentsSelectListAsync()
            };

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> AddTutor(int index)
        {
            var tutorModel = new RegisterTutorViewModel
            {
                DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync()
            };

            ViewData.TemplateInfo.HtmlFieldPrefix = $"Tutors[{index}]";

            return PartialView("_TutorForm", tutorModel);
        }

        [HttpPost]
        public async Task<IActionResult> Create(RegisterStudentViewModel model)
        {
            if (!ModelState.IsValid)
            {
                model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync();
                model.Sections = await catalogService.GetSectionsSelectListAsync();
                model.ReasonEnrollments = await catalogService.GetReasonEnrollmentsSelectListAsync();
                if (model.Tutors != null) await PopulateDocumentTypesTutorsAsync(model);

                return View(model);
            }

            var result = await studentService.CreateAsync(model);
            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred");
                model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync();
                model.Sections = await catalogService.GetSectionsSelectListAsync();
                model.ReasonEnrollments = await catalogService.GetReasonEnrollmentsSelectListAsync();
                if (model.Tutors != null) await PopulateDocumentTypesTutorsAsync(model);

                return View(model);
            }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet] public async Task<IActionResult> Edit(int id) { var model = await studentService.GetForEditAsync(id); if (model is null) return NotFound(); model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync(); return View(model); }
        [HttpPost] public async Task<IActionResult> Edit(int id, EditStudentViewModel model)
        { if (!ModelState.IsValid) { model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync(); return View(model); } var result = await studentService.UpdateAsync(id, model); if (!result.Succeeded) { ModelState.AddModelError(string.Empty, result.Error ?? "An unexpected error ocurred"); model.DocumentTypes = await catalogService.GetDocumentTypesSelectListAsync(); return View(model); } return RedirectToAction(nameof(Index)); }

        private async Task PopulateDocumentTypesTutorsAsync(RegisterStudentViewModel model)
        {
            var documentTypes = await catalogService.GetDocumentTypesSelectListAsync();

            foreach (var tutor in model.Tutors)
            {
                tutor.DocumentTypes = documentTypes;
            }
        }
    }
}
