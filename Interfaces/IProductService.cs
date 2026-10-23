using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Product;

namespace SchoolHub.Interfaces
{
    public interface IProductService
    {
        Task<List<ListProductViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterProductViewModel model);
        Task<RegisterProductViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterProductViewModel model);
    }
}
