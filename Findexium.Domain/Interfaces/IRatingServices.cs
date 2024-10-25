using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IRatingServices
    {

        Task<IEnumerable<Rating>> GetAllRatingsAsync();
        Task<Rating> GetRatingByIdAsync(int id);
        Task AddRatingAsync(Rating rating);
        Task UpdateRatingAsync(int id, Rating rating);
        Task DeleteRatingAsync(int id);
    }
}
