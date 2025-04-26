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
            try
            {
                return await _repository.GetAllAsync();
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while retrieving all curve points.", ex);
            }
        }

        public async Task<CurvePoint> GetByIdAsync(int id)
        {
            try
            {
                return await _repository.GetByIdAsync(id);
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException($"An error occurred while retrieving the curve point with ID {id}.", ex);
            }
        }

        public async Task AddAsync(CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                throw new ArgumentNullException(nameof(curvePoint));
            }

            try
            {
                await _repository.AddAsync(curvePoint);
            }
            catch (Exception ex)
            {
                
                throw new ApplicationException("An error occurred while adding a new curve point.", ex);
            }
        }

        public async Task UpdateAsync(CurvePoint curvePoint)
        {
            if (curvePoint == null)
            {
                throw new ArgumentNullException(nameof(curvePoint));
            }

            if (!await _repository.ExistsAsync(curvePoint.Id))
            {
                throw new InvalidOperationException($"Curve point with ID {curvePoint.Id} does not exist.");
            }

            try
            {
                await _repository.UpdateAsync(curvePoint.Id,curvePoint);
            }
            catch (Exception ex)
            {
               
                throw new ApplicationException($"An error occurred while updating the curve point with ID {curvePoint.Id}.", ex);
            }
        }

        public async Task DeleteAsync(int id)
        {
            if (!await _repository.ExistsAsync(id))
            {
                throw new InvalidOperationException($"Curve point with ID {id} does not exist.");
            }

            try
            {
                await _repository.DeleteAsync(id);
            }
            catch (Exception ex) when (!(ex is InvalidOperationException))
            {
               
                throw new ApplicationException($"An error occurred while deleting the curve point with ID {id}.", ex);
            }
        }
    }
}
