using SchoolHub.Common;
using SchoolHub.Models.ViewModels.Rating;

namespace SchoolHub.Interfaces
{
    public interface IRatingService
    {
        Task<List<ListRatingViewModel>> GetAllAsync();
        Task<Result> CreateAsync(RegisterRatingViewModel model);
        Task<RegisterRatingViewModel?> GetForEditAsync(int id);
        Task<Result> UpdateAsync(int id, RegisterRatingViewModel model);
    }
}
