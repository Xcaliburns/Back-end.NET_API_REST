using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;


namespace Findexium.Domain.Services
{
    public class CurvePointService : ICurvePointServices
    {
        private readonly ICurvePointRepository _repository;

        public CurvePointService(ICurvePointRepository repository)
        {
            _repository = repository;
        }

        public async Task<IEnumerable<CurvePoint>> GetAllAsync()
        {
            return await _repository.GetAllAsync();
        }

        public async Task<CurvePoint> GetByIdAsync(int id)
        {
            return await _repository.GetByIdAsync(id);
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            await _repository.AddAsync(curvePoint);
        }

        public async Task UpdateAsync(int id, CurvePoint curvePoint)
        {
            if (id != curvePoint.Id)
            {
                throw new ArgumentException("ID mismatch");
            }

            await _repository.UpdateAsync(curvePoint);
        }

        public async Task DeleteAsync(int id)
        {
            await _repository.DeleteAsync(id);
        }
    }
}
