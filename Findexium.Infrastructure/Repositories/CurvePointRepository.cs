using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Findexium.Domain.Interfaces;
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
            return await _context.CurvePoints.ToListAsync();
        }

        public async Task<CurvePoint> GetByIdAsync(int id)
        {
            return await _context.CurvePoints.FindAsync(id);
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            _context.CurvePoints.Add(curvePoint);
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
