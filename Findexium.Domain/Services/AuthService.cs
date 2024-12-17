using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Findexium.Domain.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;

        public AuthService(IAuthRepository authRepository)
        {
            _authRepository = authRepository;
        }

        public async Task<User> ValidateUserAsync(string userName, string password)
        {
            var user = await _authRepository.FindByNameAsync(userName);
            if (user != null && await _authRepository.CheckPasswordAsync(user, password))
            {
                return user;
            }
            return null;
        }

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            return await _authRepository.GetRolesAsync(user);
        }
    }
}
