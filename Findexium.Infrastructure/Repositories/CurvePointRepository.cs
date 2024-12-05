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
            try 
            {
                var curvePointsDto = await _context.CurvePoints.ToListAsync();
                return curvePointsDto.ConvertAll(c => c.ToCurvePoint());
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, "An error occurred while retrieving all curve points.");
                throw new ApplicationException("An error occurred while retrieving all curve points.", ex);
            }
          
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

            try
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
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while adding curve point.");
                throw new ApplicationException("An error occurred while adding curve point.", ex);
            }

        }

        public async Task UpdateAsync(int id, CurvePoint curvePoint)
        {
            if(curvePoint==null)
            {
                throw new ArgumentNullException(nameof(curvePoint), "Curve point cannot be null");
            }

            if (!await ExistsAsync(id))
            {
                _logger.LogWarning("CurvePoint with id: {Id} not found during update", id);
                throw new KeyNotFoundException("CurvePoint not found");
            }

            var curvePointDto = await _context.CurvePoints.FindAsync(curvePoint.Id);
            if (curvePointDto != null)
            {

                curvePointDto.CurveId = curvePoint.CurveId;
                curvePointDto.Term = curvePoint.Term;
                curvePointDto.CurvePointValue = curvePoint.CurvePointValue;
                _context.Entry(curvePointDto);
                await _context.SaveChangesAsync();

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
                throw new ApplicationException($"An error occurred while checking the existence of the CurvePoint with ID {id}", ex);
            }
        }

    }
}

