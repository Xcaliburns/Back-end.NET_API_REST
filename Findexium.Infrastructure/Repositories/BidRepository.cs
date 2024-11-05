using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
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
            var bidDtos = await _context.Bids.ToListAsync();
            return bidDtos.ConvertAll(b => b.ToBidList());
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            var bidDto = await _context.Bids.FindAsync(id);
            return bidDto.ToBidList();
        }

        public async Task AddAsync(BidList bidList)
        {
            _context.Bids.Add(new BidDto(
                bidList.BidListId,
                bidList.Account,
                bidList.BidType,
                bidList.BidQuantity,
                bidList.AskQuantity,
                bidList.Bid,
                bidList.Ask,
                bidList.Benchmark,
                bidList.BidListDate,
                bidList.Commentary,
                bidList.BidSecurity,
                bidList.BidStatus,
                bidList.Trader,
                bidList.Book,
                bidList.CreationName,
                bidList.CreationDate,
                bidList.RevisionName,
                bidList.RevisionDate,
                bidList.DealName,
                bidList.DealType,
                bidList.SourceListId,
                bidList.Side
            ));
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
