using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolHub.Interfaces
{
    public interface ICatalogService
    {
        Task<List<SelectListItem>> GetDocumentTypesSelectListAsync();
        Task<List<SelectListItem>> GetCategoryCoursesSelectListAsync();
        Task<List<SelectListItem>> GetTeachersSelectListAsync();
        Task<List<SelectListItem>> GetCoursesSelectListAsync();
        Task<List<SelectListItem>> GetSchedulesSelectListAsync();
        Task<List<SelectListItem>> GetStudentsSelectListAsync();
        Task<List<SelectListItem>> GetClassAsignmentsSelectListAsync();
        Task<List<SelectListItem>> GetShiftsSelectListAsync();
        Task<List<SelectListItem>> GetDaysSelectListAsync();
        Task<List<SelectListItem>> GetLevelsSelectListAsync();
        Task<List<SelectListItem>> GetGradesSelectListAsync();
        Task<List<SelectListItem>> GetSectionsSelectListAsync();
        Task<List<SelectListItem>> GetReasonEnrollmentsSelectListAsync();
        Task<List<SelectListItem>> GetEnrollmentsSelectListAsync();
    }
}
