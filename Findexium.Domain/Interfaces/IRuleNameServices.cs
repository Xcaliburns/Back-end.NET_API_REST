using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IRuleNameServices
    {
        Task<IEnumerable<RuleName>> GetAllRatingsAsync();
        Task<RuleName> GetRuleByIdAsync(int id);
        Task AddRuleAsync(RuleName ruleName);
        Task UpdateRuleAsync( RuleName ruleName);
        Task DeleteRuleAsync(int id);
    }
}
