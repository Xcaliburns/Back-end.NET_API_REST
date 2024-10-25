using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IRuleNameRepository
    {
        Task<IEnumerable<RuleName>> GetAllAsync();
        Task<RuleName> GetByIdAsync(int id);
        Task AddAsync(RuleName ruleName);
        Task UpdateAsync(RuleName ruleName);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
