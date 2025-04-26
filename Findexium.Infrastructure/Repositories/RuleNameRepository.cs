using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Findexium.Infrastructure.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _context;
        private readonly ILogger<RuleNameRepository> _logger;

        public RuleNameRepository(LocalDbContext context, ILogger<RuleNameRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<RuleName>> GetAllAsync()
        {
            try
            {
                var ruleNamesDtos = await _context.RuleNames.ToListAsync();
                return ruleNamesDtos.ConvertAll(r => r.ToRuleName());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving all rule names.");
                throw new Exception("An error occurred while retrieving all rule names.");
            }
        }

        public async Task<RuleName> GetByIdAsync(int id)
        {
            try
            {
                var ruleNameDto = await _context.RuleNames.FindAsync(id);
                return ruleNameDto.ToRuleName();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving rule name with id: {Id}", id);
                throw new Exception("An error occurred while retrieving the rule name.");
            }
        }

        public async Task AddAsync(RuleName ruleName)
        {
            try
            {
                _context.RuleNames.Add(new RuleNameDto(
                    ruleName.Id,
                    ruleName.Name,
                    ruleName.Description,
                    ruleName.Json,
                    ruleName.Template,
                    ruleName.SqlStr,
                    ruleName.SqlPart
                ));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new rule name.");
                throw new Exception("An error occurred while adding the rule name.");
            }
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            var ruleNameDto = await _context.RuleNames.FindAsync(ruleName.Id);
            if (ruleNameDto != null)
            {
                ruleNameDto.Name = ruleName.Name;
                ruleNameDto.Description = ruleName.Description;
                ruleNameDto.Json = ruleName.Json;
                ruleNameDto.Template = ruleName.Template;

                _context.Entry(ruleNameDto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

        }

        public async Task DeleteAsync(int id)
        {
            try
            {
                var ruleName = await _context.RuleNames.FindAsync(id);
                if (ruleName != null)
                {
                    _context.RuleNames.Remove(ruleName);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting rule name with id: {Id}", id);
                throw new Exception("An error occurred while deleting the rule name.");
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await Task.FromResult(_context.RuleNames.Any(x => x.Id == id));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if rule name with id: {Id} exists", id);
                throw new Exception("An error occurred while checking if the rule name exists.");
            }
        }
    }
}
