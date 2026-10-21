using Microsoft.AspNetCore.Mvc.Rendering;

namespace SchoolHub.Interfaces
{
    public interface ICatalogService
    {
        Task<List<SelectListItem>> GetDocumentTypesSelectListAsync();
        Task<List<SelectListItem>> GetLevelsSelectListAsync();
        Task<List<SelectListItem>> GetGradesSelectListAsync();
        Task<List<SelectListItem>> GetSectionsSelectListAsync();
        Task<List<SelectListItem>> GetReasonEnrollmentsSelectListAsync();
        Task<List<SelectListItem>> GetEnrollmentsSelectListAsync();
    }
}