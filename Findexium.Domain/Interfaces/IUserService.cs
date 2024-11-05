using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
    public interface IUserService
    {
        Task<IEnumerable<User>> GetUsersAsync();
        Task<User> GetUserByIdAsync(string id);
        Task<IdentityResult> AddUserAsync(User user, string password); // Change return type to Task<IdentityResult>
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(string id);
        Task<bool> UserExistsAsync(string id);
        Task<User> ValidateCredentialsAsync(string login, string password);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}
