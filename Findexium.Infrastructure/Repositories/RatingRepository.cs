using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Infrastructure.Repositories
{
    public class RatingRepository : IRatingRepository
    {
        private readonly LocalDbContext _context;

        public RatingRepository(LocalDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Rating>> GetAllAsync()
        {
            var ratingDtos = await _context.Ratings.ToListAsync();
            return ratingDtos.ConvertAll(r => r.ToRating());
        }

        public async Task<Rating> GetByIdAsync(int id)
        {
            var ratingDto = await _context.Ratings.FindAsync(id);
            return ratingDto.ToRating();
        }

        public async Task AddAsync(Rating rating)
        {
            _context.Ratings.Add(new RatingDto(
                rating.Id,
                rating.MoodysRating,
                rating.SandPRating, rating.FitchRating,
                rating.OrderNumber
                ));
            await _context.SaveChangesAsync();
        }

        public async Task UpdateAsync(Rating rating)
        {
            var ratingDto = await _context.Ratings.FindAsync(rating.Id);
            if (ratingDto != null)
            {
                // Modification for the test _context.Entry(rating).State = EntityState.Modified;
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
            return await _context.Ratings.AnyAsync(e => e.Id == id);
        }
    }
}

