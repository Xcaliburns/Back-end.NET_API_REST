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
            try
            {
                var bidDtos = await _context.Bids.ToListAsync();
                return bidDtos.ConvertAll(b => b.ToBidList());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all bids.");
                throw new ApplicationException("An error occurred while retrieving all bids.", ex);
            }
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            
            try
            {
                var bidDto = await _context.Bids.FindAsync(id);
                if (bidDto == null)
                {
                    return null;
                }
                return bidDto.ToBidList();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving the bid with ID {Id}.", id);
                throw new Exception($"An error occurred while retrieving the bid with ID {id}.", ex);
            }
        }

        public async Task AddAsync(BidList bidList)
        {
            try
            {
                _context.Bids.Add(new BidDto(
                    bidList.Account,
                    bidList.BidType,
                    bidList.BidQuantity,
                    bidList.AskQuantity,
                    bidList.Bid ?? 0.0,
                    bidList.Ask,
                    bidList.Benchmark,
                    bidList.BidListDate,
                    bidList.Commentary,
                    bidList.BidSecurity,
                    bidList.BidStatus,
                    bidList.Trader,
                    bidList.Book,
                    bidList.CreationName,
                    bidList.CreationDate ?? DateTime.Now,
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
                _logger.LogError(ex, "An error occurred while adding the bid.");
                throw new ApplicationException("An error occurred while adding the bid.", ex);
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
           
                var existingBid = await _context.Bids.FindAsync(id);
            if (existingBid != null)
            {
                // Update properties
                existingBid.Account = bidList.Account;
                existingBid.BidType = bidList.BidType;
                existingBid.BidQuantity = bidList.BidQuantity;
                existingBid.RevisionDate = DateTime.Now;
                _context.Bids.Update(existingBid);
                await _context.SaveChangesAsync();
            }     
          
        }

        public async Task DeleteAsync(int id)
        {
            //le catch est dans ExistsAsync
            if (await ExistsAsync(id))
            {
                var bidList = await _context.Bids.FindAsync(id);
                _context.Bids.Remove(bidList);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Bids.AnyAsync(e => e.BidListId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking the existence of the bid with ID {Id}.", id);
                throw new ApplicationException($"An error occurred while checking the existence of the bid with ID {id}.", ex);
            }
        }
    }
}
