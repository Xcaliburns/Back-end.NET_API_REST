using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<bool> UserExistsAsync(string id);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<User> GetUserByUserNameAsync(string userName);
      
    }
}
