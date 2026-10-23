using SchoolHub.Common;
using SchoolHub.Models.ViewModels.DocumentType;

namespace SchoolHub.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<List<ListDocumentTypeViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, bool isActive);
        Task<RegisterDocumentTypeViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, string name, bool isActive);
    }
}
