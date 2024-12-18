using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{
    public interface IAuthRepository
    {
        Task<User> FindByNameAsync(string userName);
        Task<bool> CheckPasswordAsync(User user, string password);
        Task<IList<string>> GetRolesAsync(User user); // Add this method
    }
}
