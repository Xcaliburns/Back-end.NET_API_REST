using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Infrastructure.Repositories
{
    public class RuleNameRepository : IRuleNameRepository
    {
        private readonly LocalDbContext _context;

        public RuleNameRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<RuleName>> GetAllAsync()
        {
           var ruleNamesDtos = await _context.RuleNames.ToListAsync();
            return ruleNamesDtos.ConvertAll(r => r.ToRuleName()); 

        }

        public async Task<RuleName> GetByIdAsync(int id)
        {
            var ruleNameDto = await _context.RuleNames.FindAsync(id);
            return ruleNameDto.ToRuleName();
        }

        public async Task AddAsync(RuleName ruleName)
        {
           _context.RuleNames.Add(new RuleNameDto(
                ruleName.Name,
                ruleName.Description,
                ruleName.Json,
                ruleName.Template,
                ruleName.SqlStr,
                ruleName.SqlPart
            ));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            _context.Entry(ruleName).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ruleName = await _context.RuleNames.FindAsync(id);
            if(ruleName!=null)
                {
                _context.RuleNames.Remove(ruleName);
                await _context.SaveChangesAsync();
            }
           
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(_context.RuleNames.Any(x => x.Id == id));
        }
    }
}
