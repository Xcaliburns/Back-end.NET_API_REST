using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Findexium.Infrastructure.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {
        private readonly LocalDbContext _context;
        private readonly ILogger<CurvePointRepository> _logger;

        public CurvePointRepository(LocalDbContext context, ILogger<CurvePointRepository> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllAsync()
        {
            var curvePointsDto = await _context.CurvePoints.ToListAsync();
            return curvePointsDto.ConvertAll(c => c.ToCurvePoint());
        }

        public async Task<CurvePoint> GetByIdAsync(int id)
        {
            try
            {
                var curvePointDto = await _context.CurvePoints.FindAsync(id);
                if (curvePointDto == null)
                {
                    return null;
                }
                return curvePointDto.ToCurvePoint();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error occurred while fetching curve point with id: {id}", ex);
            }
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            _context.CurvePoints.Add(new CurvePointsDto(
                curvePoint.CurveId,
                curvePoint.AsOfDate,
                curvePoint.Term,
                curvePoint.CurvePointValue,
                curvePoint.CreationDate
            ));
            await _context.SaveChangesAsync();
        }

        public async Task<bool> UpdateAsync(int id, CurvePoint curvePoint)
        {
            var curvePointDto = await _context.CurvePoints.FindAsync(curvePoint.Id);
            if (curvePointDto == null)
            {
                return false;
            }

            curvePointDto.CurveId = curvePoint.CurveId;
            curvePointDto.Term = curvePoint.Term;
            curvePointDto.CurvePointValue = curvePoint.CurvePointValue;
            curvePointDto.CreationDate = curvePoint.CreationDate;

            _context.Entry(curvePointDto).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException ex)
            {
                _logger.LogError(ex, "Concurrency error occurred while updating curvePoint with id: {Id}", id);

                throw;

            }
        }

        public async Task DeleteAsync(int id)
        {
            var curvePoint = await _context.CurvePoints.FindAsync(id);
            if (curvePoint != null)
            {
                _context.CurvePoints.Remove(curvePoint);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            try
            {
                return await _context.CurvePoints.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while checking if curve point with id: {Id} exists", id);
                throw;
            }
        }

    }
}

