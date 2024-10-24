using Dot.Net.WebApi.Domain;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace P7CreateRestApi.Interfaces
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
