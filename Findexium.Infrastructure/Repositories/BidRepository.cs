using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Microsoft.EntityFrameworkCore;
using P7CreateRestApi.Interfaces;

namespace P7CreateRestApi.Repositories
{
    public class BidRepository : IBidListRepository
    { 
        private readonly LocalDbContext _context;

        public BidRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            return await _context.Bids.ToListAsync();
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            return await _context.Bids.FindAsync(id);
        }

        public async Task AddAsync(BidList bidList)
        {
            _context.Bids.Add(bidList);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(BidList bidList)
        {
            _context.Entry(bidList).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var bidList = await _context.Bids.FindAsync(id);
            if (bidList != null)
            {
                _context.Bids.Remove(bidList);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bids.AnyAsync(e => e.BidListId == id);
        }
    }
}
