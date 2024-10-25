using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Findexium.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Findexium.Infrastructure.Repositories.RatingRepository;

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
                return await _context.Rating.ToListAsync();
            }

            public async Task<Rating> GetByIdAsync(int id)
            {
                return await _context.Rating.FindAsync(id);
            }

            public async Task AddAsync(Rating rating)
            {
                _context.Rating.Add(rating);
                await _context.SaveChangesAsync();
            }

            public async Task UpdateAsync(Rating rating)
            {
                _context.Entry(rating).State = EntityState.Modified;
                await _context.SaveChangesAsync();
            }

            public async Task DeleteAsync(int id)
            {
                var rating = await _context.Rating.FindAsync(id);
                if (rating != null)
                {
                    _context.Rating.Remove(rating);
                    await _context.SaveChangesAsync();
                }
            }

            public async Task<bool> ExistsAsync(int id)
            {
                return await _context.Rating.AnyAsync(e => e.Id == id);
            }
        }
    }

