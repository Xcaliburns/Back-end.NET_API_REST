using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;

namespace Findexium.Domain.Services
{
    public class TradeService : ITradeService
    {
        private readonly ITradeRepository _tradeRepository;

        public TradeService(ITradeRepository tradeRepository)
        {
            _tradeRepository = tradeRepository;
        }

        public async Task<IEnumerable<Trade>> GetAllTradesAsync()
        {
            return await _tradeRepository.GetAllTradesAsync();
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            return await _tradeRepository.GetTradeByIdAsync(id);
        }

        public async Task AddTradeAsync(Trade trade)
        {
            await _tradeRepository.AddTradeAsync(trade);
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            await _tradeRepository.UpdateTradeAsync(trade);
        }

        public async Task DeleteTradeAsync(int id)
        {
            await _tradeRepository.DeleteTradeAsync(id);
        }

        public async Task<bool> TradeExistsAsync(int id)
        {
            return await _tradeRepository.TradeExistsAsync(id);
        }
    }
}
