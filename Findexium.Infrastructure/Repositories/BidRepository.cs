using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Infrastructure.Repositories
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
            using (var transaction = await _context.Database.BeginTransactionAsync())
            {
                try
                {
                    //TODO : voir avec Laala si les requetes avec swagger sont ok
                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Bids ON");

                    _context.Bids.Add(bidList);
                    await _context.SaveChangesAsync();

                    await _context.Database.ExecuteSqlRawAsync("SET IDENTITY_INSERT Bids OFF");
                    await transaction.CommitAsync();
                }
                catch (Exception e)
                {
                    await transaction.RollbackAsync();
                    // Log the exception or handle it as needed
                    throw;
                }
            }
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
