using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface IRatingServices
    {

        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating> GetRatingByIdAsync(int id);
        Task AddRatingAsync(Rating rating);
        Task UpdateRatingAsync( Rating rating);
        Task DeleteRatingAsync(int id);
    }
}
