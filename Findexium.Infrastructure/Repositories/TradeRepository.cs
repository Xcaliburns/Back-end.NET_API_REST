using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Repositories
{
    public class TradeRepository : ITradeRepository
    {
        private readonly LocalDbContext _context;
        private readonly ILogger<TradeRepository> _logger;

        public TradeRepository(LocalDbContext context,ILogger<TradeRepository> logger )
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            var tradeDtos= await _context.Trades.ToListAsync();
            return tradeDtos.ConvertAll(t => t.ToTrade());
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            var tradeDto= await _context.Trades.FindAsync(id);
            return tradeDto.ToTrade();
        }

        public async Task AddTradeAsync(Trade trade)
        {
           _context.Trades.Add(new TradeDto(
               trade.Account,
               trade.AccountType,
               trade.BuyQuantity,
               trade.SellQuantity,
               trade.BuyPrice,
               trade.SellPrice,
               trade.TradeDate,
               trade.TradeSecurity,
               trade.TradeStatus,
               trade.Trader,
               trade.Benchmark,
               trade.Book,
               trade.CreationName,
               trade.CreationDate,
               trade.RevisionName,
               trade.RevisionDate,
               trade.DealName
            ));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            if (trade == null)
            {
                throw new ArgumentNullException(nameof(trade), "Trade cannot be null");
            }

            if (!await TradeExistsAsync(trade.TradeId))
            {
                _logger.LogWarning("Trade with id: {Id} not found", trade.TradeId);
                throw new KeyNotFoundException("Trade not found");
            }

            try
            {
                var tradeDto = new TradeDto(
                    trade.Account,
                    trade.AccountType,
                    trade.BuyQuantity,
                    trade.SellQuantity,
                    trade.BuyPrice,
                    trade.SellPrice,
                    trade.TradeDate,
                    trade.TradeSecurity,
                    trade.TradeStatus,
                    trade.Trader,
                    trade.Benchmark,
                    trade.Book,
                    trade.CreationName,
                    trade.CreationDate,
                    trade.RevisionName,
                    trade.RevisionDate,
                    trade.DealName
                )
                {
                    TradeId = trade.TradeId
                };

                _context.Entry(tradeDto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                throw;
            }
        }

        public async Task DeleteTradeAsync(int id)
        {
            var trade = await _context.Trades.FindAsync(id);
            if (trade != null)
            {
                _context.Trades.Remove(trade);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> TradeExistsAsync(int id)
        {
            return await _context.Trades.AnyAsync(e => e.TradeId == id);
        }
    }
}
