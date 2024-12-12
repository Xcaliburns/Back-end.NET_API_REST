using Findexium.Domain.Models;

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
