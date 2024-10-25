using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Findexium.Domain.Interfaces;
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
            return await _context.Trades.ToListAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _context.Trades.FirstOrDefaultAsync(m => m.TradeId == id);
        }

        public async Task AddTradeAsync(Trade trade)
        {
            _context.Trades.Add(trade);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            _context.Trades.Update(trade);
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
