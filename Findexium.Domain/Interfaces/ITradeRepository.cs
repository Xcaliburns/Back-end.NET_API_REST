using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface ITradeRepository
    {
        Task<IEnumerable<Trade>> GetAllTradesAsync();
        Task<Trade> GetTradeByIdAsync(int id);
        Task AddTradeAsync(Trade trade);
        Task UpdateTradeAsync(Trade trade);
        Task DeleteTradeAsync(int id);
        Task<bool> TradeExistsAsync(int id);
    }
}
