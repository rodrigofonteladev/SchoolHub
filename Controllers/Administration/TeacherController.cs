using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SchoolHub.Common.Enums;
using SchoolHub.Interfaces;

namespace SchoolHub.Controllers.Administration
{
    [Authorize(Roles = nameof(Role.Admin))]
    public class TeacherController(ITeacherService teacherService) : Controller
    {
        public async Task<IActionResult> Index()
        {
            return View(await teacherService.GetAllAsync());
        }
    }
}
