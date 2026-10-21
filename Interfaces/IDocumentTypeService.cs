using SchoolHub.Common;
using SchoolHub.Models.ViewModels.DocumentType;

namespace SchoolHub.Interfaces
{
    public interface IDocumentTypeService
    {
        Task<List<ListDocumentTypeViewModel>> GetAllAsync();
        Task<Result> CreateAsync(string name, bool isActive);
    }
}