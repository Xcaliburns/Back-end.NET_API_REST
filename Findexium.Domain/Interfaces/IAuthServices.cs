using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity.Data;
using System.Threading.Tasks;

namespace Findexium.Domain.Interfaces
{
   
    
public interface IAuthService
    {
        Task<User> ValidateUserAsync(string userName, string password);
        Task<IList<string>> GetUserRolesAsync(User user);
    }
}
