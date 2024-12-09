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
            try
            {
                return await _tradeRepository.GetAllTradesAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all trades.", ex);
            }
        }

        public async Task<Trade> GetTradeByIdAsync(int id)
        {
            try
            {
                return await _tradeRepository.GetTradeByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the trade with ID {id}.", ex);
            }
        }

        public async Task AddTradeAsync(Trade trade)
        {
            if (trade == null)
            {
                throw new ArgumentNullException(nameof(trade));
            }

            try
            {
                await _tradeRepository.AddTradeAsync(trade);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new trade.", ex);
            }
        }

        public async Task UpdateTradeAsync(Trade trade)
        {
            if (trade == null)
            {
                throw new ArgumentNullException(nameof(trade));
            }

            if (!await _tradeRepository.TradeExistsAsync(trade.TradeId))
            {
                throw new InvalidOperationException($"Trade with ID {trade.TradeId} does not exist.");
            }

            try
            {
                await _tradeRepository.UpdateTradeAsync(trade);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the trade with ID {trade.TradeId}.", ex);
            }
        }

        public async Task DeleteTradeAsync(int id)
        {

            if (!await _tradeRepository.TradeExistsAsync(id))
            {
                throw new InvalidOperationException($"Trade with ID {id} does not exist.");
            }
            try
            {
               

                await _tradeRepository.DeleteTradeAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while deleting the trade with ID {id}.", ex);
            }
        }

        public async Task<bool> TradeExistsAsync(int id)
        {
            try
            {
                return await _tradeRepository.TradeExistsAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while checking if the trade with ID {id} exists.", ex);
            }
        }
    }
}
