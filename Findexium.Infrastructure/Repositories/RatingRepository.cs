using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Findexium.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _context;
        private readonly ILogger<RatingRepository> _logger;

        public RatingRepository(LocalDbContext context, ILogger<RatingRepository> logger)
        {
            _context = context; 
            _logger = logger;
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            try
            {
                var ratingDtos = await _context.Ratings.ToListAsync();
                return ratingDtos.ConvertAll(r => r.ToRating());
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving all ratings.", ex);
            } 
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            try
            {
                var ratingDto = await _context.Ratings.FindAsync(id);
            return ratingDto.ToRating();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while retrieving the rating.", ex);
            }
        }

        public async Task AddAsync(Rating rating)
        {
            try
            {
                _context.Ratings.Add(new RatingDto(
              rating.Id,
              rating.MoodysRating,
              rating.SandPRating, rating.FitchRating,
              rating.OrderNumber
              ));
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                throw new Exception("An error occurred while adding the rating.", ex);
            }
          
        }

        public async Task UpdateAsync(Rating rating)
        {
            var ratingDto = await _context.Ratings.FindAsync(rating.Id);
            if (ratingDto != null)
            {
                
                ratingDto.MoodysRating = rating.MoodysRating;
                ratingDto.SandPRating = rating.SandPRating;
                ratingDto.FitchRating = rating.FitchRating;
                ratingDto.OrderNumber = rating.OrderNumber;

                _context.Entry(ratingDto).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }
        }

        public async Task DeleteAsync(int id)
        {
            var rating = await _context.Ratings.FindAsync(id);
            if (rating != null)
            {
                _context.Ratings.Remove(rating);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.Ratings.AnyAsync(e => e.Id == id);
            }
            catch
            {
                throw new Exception("An error occurred while checking if the rating exists.");
            }
        }
    }
}

