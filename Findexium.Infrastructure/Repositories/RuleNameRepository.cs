using Findexium.Domain.Interfaces;
using Dot.Net.WebApi.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.WebApi.Controllers;

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
            return await Task.FromResult(_context.RuleNames.AsEnumerable());
        }

        public async Task<RuleName> GetByIdAsync(int id)
        {
            return await Task.FromResult(_context.RuleNames.FirstOrDefault(x => x.Id == id));
        }

        public async Task AddAsync(RuleName ruleName)
        {
            await _context.RuleNames.AddAsync(ruleName);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(RuleName ruleName)
        {
            _context.RuleNames.Update(ruleName);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var ruleName = await GetByIdAsync(id);
            _context.RuleNames.Remove(ruleName);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await Task.FromResult(_context.RuleNames.Any(x => x.Id == id));
        }
    }
}
