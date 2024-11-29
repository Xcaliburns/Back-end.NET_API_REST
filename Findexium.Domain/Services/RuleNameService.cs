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

        public async Task<IEnumerable<RuleName>> GetAllRulesAsync()
        {
            try
            {
                return await _ruleNameRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all rule names.", ex);
            }
        }

        public async Task<RuleName> GetRuleByIdAsync(int id)
        {
            try
            {
                return await _ruleNameRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the rule name with ID {id}.", ex);
            }
        }

        public async Task AddRuleAsync(RuleName ruleName)
        {
            if (ruleName == null)
            {
                throw new ArgumentNullException(nameof(ruleName));
            }

            try
            {
                await _ruleNameRepository.AddAsync(ruleName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new rule name.", ex);
            }
        }

        public async Task UpdateRuleAsync(RuleName ruleName)
        {
            if (ruleName == null)
            {
                throw new ArgumentNullException(nameof(ruleName));
            }

            if (!await _ruleNameRepository.ExistsAsync(ruleName.Id))
            {
                throw new InvalidOperationException($"Rule name with ID {ruleName.Id} does not exist.");
            }

            try
            {
                await _ruleNameRepository.UpdateAsync(ruleName);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the rule name with ID {ruleName.Id}.", ex);
            }
        }

        public async Task DeleteRuleAsync(int id)
        {
            if (!await _ruleNameRepository.ExistsAsync(id))
            {
                throw new InvalidOperationException($"Rule name with ID {id} does not exist.");
            }

            try
            {
                await _ruleNameRepository.DeleteAsync(id);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                throw new ApplicationException($"An error occurred while deleting the rule name with ID {id}.", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _ruleNameRepository.ExistsAsync(id);
        }
    }
}
