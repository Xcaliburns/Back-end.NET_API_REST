using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;

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
            try
            {
                return await _ratingRepository.GetAllAsync();
            }
            catch (Exception ex)
            {
               
                throw new Exception("An error occurred while retrieving all ratings.", ex);
            }
        }

        public async Task<Rating> GetRatingByIdAsync(int id)
        {
            try
            {
                return await _ratingRepository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
              
                throw new Exception($"An error occurred while retrieving the rating with id {id}.", ex);
            }
        }

        public async Task AddRatingAsync(Rating rating)
        {
            try
            {
                await _ratingRepository.AddAsync(rating);
            }
            catch (Exception ex)
            {
                
                throw new Exception("An error occurred while adding a new rating.", ex);
            }
        }

        public async Task UpdateRatingAsync(Rating rating)
        {
            try
            {
                await _ratingRepository.UpdateAsync(rating);
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while updating the rating with id {rating.Id}.", ex);
            }
        }

        public async Task DeleteRatingAsync(int id)
        {
            try
            {
                await _ratingRepository.DeleteAsync(id);
            }
            catch (Exception ex)
            {
                
                throw new Exception($"An error occurred while deleting the rating with id {id}.", ex);
            }
        }
    }
}
