using Dot.Net.WebApi.Domain;
using Findexium.Domain.Interfaces;
using P7CreateRestApi.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Services
{
    public class BidListServices : IBidListServices
    {
        private readonly IBidListRepository _bidRepository;

        public BidListServices(IBidListRepository bidRepository)
        {
            _bidRepository = bidRepository;
        }

        public async Task<IEnumerable<BidList>> GetAllAsync()
        {
            return await _bidRepository.GetAllAsync();
        }

        public async Task<BidList> GetByIdAsync(int id)
        {
            return await _bidRepository.GetByIdAsync(id);
        }

        public async Task AddAsync(BidList bidList)
        {
            await _bidRepository.AddAsync(bidList);
        }

        public async Task UpdateAsync(BidList bidList)
        {
            await _bidRepository.UpdateAsync(bidList);
        }

        public async Task DeleteAsync(int id)
        {
            await _bidRepository.DeleteAsync(id);
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _bidRepository.ExistsAsync(id);
        }
    }
}
