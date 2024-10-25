using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Services
{
    public class RatingService : IRatingServices
    {
        private readonly IRatingRepository _ratingRepository;

        public RatingService(IRatingRepository ratingRepository)
        {
            _ratingRepository = ratingRepository;
        }

        public async Task<IEnumerable<Rating>> GetAllRatingsAsync()
        {
            return await _ratingRepository.GetAllAsync();
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            return await _ratingRepository.GetByIdAsync(id);
        }

        public async Task AddRatingAsync(Rating rating)
        {
            await _ratingRepository.AddAsync(rating);
        }

        public async Task UpdateRatingAsync(int id, Rating rating)
        {
            if (id != rating.Id)
            {
                throw new ArgumentException("ID mismatch");
            }

            await _ratingRepository.UpdateAsync(rating);
        }

        public async Task DeleteRatingAsync(int id)
        {
            await _ratingRepository.DeleteAsync(id);
        }
    }
}
