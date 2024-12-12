using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface IRatingRepository
    {
        Task<IEnumerable<Rating>> GetAllAsync();
        Task<Rating> GetByIdAsync(int id);
        Task AddAsync(Rating rating);
        Task UpdateAsync(Rating rating);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
