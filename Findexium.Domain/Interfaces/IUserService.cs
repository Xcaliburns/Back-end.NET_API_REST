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
        Task<IdentityResult> AddUserAsync(User user, string password);
        Task<IdentityResult> UpdateUserAsync(User user);
        Task<IdentityResult> DeleteUserAsync(string id);
        Task<bool> UserExistsAsync(string id);
        Task<User> ValidateCredentialsAsync(string login, string password);
        Task<IList<string>> GetUserRolesAsync(User user);
        Task<User> GetUserByUserNameAsync(string userName); // Nouvelle méthode
    }
}
