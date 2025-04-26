using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface IRuleNameServices
    {
        Task<IEnumerable<RuleName>> GetAllRulesAsync();
        Task<RuleName> GetRuleByIdAsync(int id);
        Task AddRuleAsync(RuleName ruleName);
        Task UpdateRuleAsync( RuleName ruleName);
        Task DeleteRuleAsync(int id);
    }
}
