using Findexium.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IBidListRepository
    {
        Task<IEnumerable<BidList>> GetAllAsync();
        Task<BidList> GetByIdAsync(int id);
        Task AddAsync(BidList bidList);
        Task UpdateAsync(BidList bidList);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
