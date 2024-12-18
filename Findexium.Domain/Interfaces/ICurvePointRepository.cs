using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface ICurvePointRepository
    {
        Task<IEnumerable<CurvePoint>> GetAllAsync();
        Task<CurvePoint> GetByIdAsync(int id);
        Task AddAsync(CurvePoint curvePoint);
        Task UpdateAsync(int id,CurvePoint curvePoint); 
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
    }
}
