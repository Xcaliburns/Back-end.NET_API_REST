using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;

namespace Findexium.Domain.Services
{
    public class BidListService : IBidListServices
    {
        private readonly IBidListRepository _bidRepository;

        public BidListService(IBidListRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            try
            {
                return await _bidRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while retrieving all bid lists.", ex);
            }
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            try
            {
                return await _bidRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while retrieving the bid list with ID {id}.", ex);
            }
        }
      

        public async Task AddAsync(BidList bidList)
        {
            if (bidList == null)
            {
                throw new ArgumentNullException(nameof(bidList));
            }

            try
            {
                await _bidRepository.AddAsync(bidList);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("An error occurred while adding a new bid list.", ex);
            }
        }

        public async Task UpdateAsync(int Id,BidList bidList)
        {
            if (bidList == null)
            {
                throw new ArgumentNullException(nameof(bidList));
            }

            if (!await _bidRepository.ExistsAsync(Id))
            {
                throw new InvalidOperationException($"Bid list with ID {Id} does not exist.");
            }

            try
            {
                await _bidRepository.UpdateAsync(Id,bidList);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while updating the bid list with ID {bidList.BidListId}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _bidRepository.ExistsAsync(id))
            {
                throw new InvalidOperationException($"Bid list with ID {id} does not exist.");
            }

            try
            {
                await _bidRepository.DeleteAsync(id);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
                throw new ApplicationException($"An error occurred while deleting the bid list with ID {id}.", ex);
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _bidRepository.ExistsAsync(id);
            }
            catch (Exception ex)
            {
                throw new ApplicationException($"An error occurred while checking the existence of the bid list with ID {id}.", ex);
            }
        }
    }
}
