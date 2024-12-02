using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
