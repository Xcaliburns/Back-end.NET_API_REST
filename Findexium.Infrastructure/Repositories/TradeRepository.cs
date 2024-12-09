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

        public TradeRepository(LocalDbContext context, ILogger<TradeRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            try
            {
                var tradeDtos = await _context.Trades.ToListAsync();
                return tradeDtos.ConvertAll(t => t.ToTrade());
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all trades.");
                throw new Exception("An error occurred while retrieving all trades.", ex);
            }
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            try
            {
                var tradeDto = await _context.Trades.FindAsync(id);
                return tradeDto.ToTrade();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving trade with id: {Id}", id);
                throw new Exception("An error occurred while retrieving the trade.");

            }
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
            var tradeDto = await _context.Trades.FindAsync(trade.TradeId);
            if (trade != null)
            {

                tradeDto.Account = trade.Account;
                tradeDto.AccountType = trade.AccountType;
                tradeDto.BuyQuantity = trade.BuyQuantity;
                tradeDto.SellQuantity = trade.SellQuantity;
                tradeDto.BuyPrice = trade.BuyPrice;
                tradeDto.SellPrice = trade.SellPrice;
                tradeDto.TradeDate = trade.TradeDate;
                tradeDto.TradeSecurity = trade.TradeSecurity;
                tradeDto.TradeStatus = trade.TradeStatus;
                tradeDto.Trader = trade.Trader;
                tradeDto.Benchmark = trade.Benchmark;
                tradeDto.Book = trade.Book;
                tradeDto.CreationName = trade.CreationName;
                tradeDto.CreationDate = trade.CreationDate;
                tradeDto.RevisionName = trade.RevisionName;
                tradeDto.RevisionDate = trade.RevisionDate;
                tradeDto.DealName = trade.DealName;

                {
                    tradeDto.TradeId = trade.TradeId;
                };

                _context.Entry(tradeDto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }           
        }

        public async Task DeleteTradeAsync(int id)
        {
            try
            {
                var trade = await _context.Trades.FindAsync(id);
                if (trade != null)
                {
                    _context.Trades.Remove(trade);
                    await _context.SaveChangesAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while deleting the trade with id: {Id}", id);
                throw new Exception("An error occurred while deleting the trade.", ex);
            }
        }

        public async Task<bool> TradeExistsAsync(int id)
        {
            try
            {
                return await _context.Trades.AnyAsync(e => e.TradeId == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if trade exists");
                throw new Exception("An error occurred while checking if trade exists");
            }

        }
    }
}
