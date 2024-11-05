using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
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

        public TradeRepository(LocalDbContext context)
        {
            _context = context;
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
          _context.Entry(trade).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
