using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface IBidListServices
    {
        Task<IEnumerable<BidList>> GetAllAsync();
        Task<BidList> GetByIdAsync(int id);
        Task AddAsync(BidList bidList);
        Task UpdateAsync(int Id,BidList bidList);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
