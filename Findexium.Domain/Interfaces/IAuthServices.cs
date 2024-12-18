using Findexium.Domain.Models;

namespace Findexium.Domain.Interfaces
{


    public interface IAuthService
    {
        Task<User> ValidateUserAsync(string userName, string password);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}
