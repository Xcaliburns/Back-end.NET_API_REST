using Findexium.Domain.Models;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user); // Add this method
    }
}
