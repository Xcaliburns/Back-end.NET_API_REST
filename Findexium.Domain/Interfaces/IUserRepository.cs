using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task AddUserAsync(User user);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<bool> UserExistsAsync(string id);
        Task<IdentityResult> AddUserToRoleAssync(User user, string role);
    }
}
