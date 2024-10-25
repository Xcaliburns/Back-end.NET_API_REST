using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Services
{
    public class RuleNameService : IRuleNameServices
    {
        private readonly IRuleNameRepository _ruleNameRepository;

        public RuleNameService(IRuleNameRepository ruleNameRepository)
        {
            _ruleNameRepository = ruleNameRepository;
        }

        public async Task<IEnumerable<RuleName>> GetAllRatingsAsync()
        {
            return await _ruleNameRepository.GetAllAsync();
        }
        public async Task<RuleName> GetRuleByIdAsync(int id)
        {
            return await _ruleNameRepository.GetByIdAsync(id);
        }
        public async Task AddRuleAsync(RuleName ruleName)
        {
            await _ruleNameRepository.AddAsync(ruleName);
        }

        public async Task UpdateRuleAsync(int id, RuleName ruleName)
        {
            if (id != ruleName.Id)
            {
                throw new ArgumentException("ID mismatch");
            }

            await _ruleNameRepository.UpdateAsync(ruleName);
        }

        public async Task DeleteRuleAsync(int id)
        {
            await _ruleNameRepository.DeleteAsync(id);
        }
    }
}
