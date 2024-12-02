using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Findexium.Infrastructure.Repositories
{
    public class BidRepository : IBidListRepository
    {
        private readonly LocalDbContext _context;
        private readonly ILogger<BidRepository> _logger;

        public BidRepository(LocalDbContext context, ILogger<BidRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            var bidDtos = await _context.Bids.ToListAsync();
            return bidDtos.ConvertAll(b => b.ToBidList());
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            var bidDto = await _context.Bids.FindAsync(id);
            if (bidDto == null)
            {
                return null;
            }
            return bidDto.ToBidList();
        }

        public async Task AddAsync(BidList bidList)
        {
            try
            {
                _context.Bids.Add(new BidDto(
                  //  bidList.BidListId,
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
            catch (Exception ex)
            {
                // Log the exception
                throw new Exception("An error occurred while adding the bid.", ex);
            }
        }

        public async Task UpdateAsync(int id, BidList bidList)
        {
            if (bidList == null)
            {
                throw new ArgumentNullException(nameof(bidList), "Bid cannot be null");
            }
            if (!await ExistsAsync(id))
            {
                _logger.LogWarning("Bid with id: {Id} not found during update", id);
                throw new KeyNotFoundException("Bid not found");
            }
            try
            {
                var existingBid = await _context.Bids.FindAsync(id);
                if (existingBid != null)
                {
                    // Update properties
                    existingBid.Account = bidList.Account;
                    existingBid.BidType = bidList.BidType;
                    existingBid.BidQuantity = bidList.BidQuantity;
                    existingBid.AskQuantity = bidList.AskQuantity;
                    existingBid.Bid = bidList.Bid;
                    existingBid.Ask = bidList.Ask;
                    existingBid.Benchmark = bidList.Benchmark;
                    existingBid.BidListDate = bidList.BidListDate;
                    existingBid.Commentary = bidList.Commentary;
                    existingBid.BidSecurity = bidList.BidSecurity;
                    existingBid.BidStatus = bidList.BidStatus;
                    existingBid.Trader = bidList.Trader;
                    existingBid.Book = bidList.Book;
                    existingBid.CreationName = bidList.CreationName;
                    existingBid.CreationDate = bidList.CreationDate;
                    existingBid.RevisionName = bidList.RevisionName;
                    existingBid.RevisionDate = bidList.RevisionDate;
                    existingBid.DealName = bidList.DealName;
                    existingBid.DealType = bidList.DealType;
                    existingBid.SourceListId = bidList.SourceListId;
                    existingBid.Side = bidList.Side;

                    _context.Bids.Update(existingBid);
                    await _context.SaveChangesAsync();
                }
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while updating bid with id: {Id}", id);
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while updating bid with id: {Id}", id);
                throw;
            }
        }

        public async Task DeleteAsync(int id)
        {
            var bidList = await _context.Bids.FindAsync(id) ?? throw new Exception("Bid not found.");
            _context.Bids.Remove(bidList);
            await _context.SaveChangesAsync();
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Bids.AnyAsync(e => e.BidListId == id);
        }
         
    }
}
