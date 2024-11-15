using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Findexium.Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Infrastructure.Repositories
{
    public class CurvePointRepository : ICurvePointRepository
    {
        private readonly LocalDbContext _context;

        public CurvePointRepository(LocalDbContext context)
        {
            _context = context;
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
                curvePoint.CurveId ?? 0, // Provide a default value if CurveId is null
                curvePoint.AsOfDate ?? DateTime.Now, // Provide a default value if AsOfDate is null
                curvePoint.Term ?? 0, // Provide a default value if Term is null
                curvePoint.CurvePointValue ?? 0, // Provide a default value if CurvePointValue is null
                curvePoint.CreationDate ?? DateTime.Now // Provide a default value if CreationDate is null
            ));
            await _context.SaveChangesAsync();
            
        }

        public async Task UpdateAsync(CurvePoint curvePoint)
        {
            _context.Entry(curvePoint).State = EntityState.Modified;
            await _context.SaveChangesAsync();
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
            return await _context.CurvePoints.AnyAsync(e => e.Id == id);
        }
    }
}
