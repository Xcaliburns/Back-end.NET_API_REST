using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;

namespace Findexium.Domain.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UserService> _logger;

        public UserService(IUserRepository userRepository, UserManager<User> userManager, ILogger<UserService> logger)
        {
            _userRepository = userRepository;
            _userManager = userManager;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                return await _userRepository.GetUsersAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users");
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                return await _userRepository.GetUserByIdAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by id: {Id}", id);
                throw;
            }
        }

        public async Task<User> GetUserByUserNameAsync(string userName)
        {
            return await _userManager.FindByNameAsync(userName);
        }

        public async Task<IdentityResult> AddUserAsync(User user, string password)
        {
            var existingUser = await GetUserByUserNameAsync(user.UserName);
            if (existingUser != null)
            {
                return IdentityResult.Failed(new IdentityError { Description = "Username already exists." });
            }

           
            try
            {
                user.PasswordHash = password; // Set the password hash
                await _userRepository.AddUserAsync(user);
                return IdentityResult.Success;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new user");
                return IdentityResult.Failed(new IdentityError { Description = "An error occurred while adding the user." });
            }
        }

        public async Task<IdentityResult> UpdateUserAsync(User user)
        {
            try
            {
                return await _userManager.UpdateAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw;
            }
        }

        public async Task<IdentityResult> DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                if (user == null)
                {
                    return IdentityResult.Failed(new IdentityError { Description = "User not found" });
                }

                return await _userManager.DeleteAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with id: {Id}", id);
                throw;
            }
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            try
            {
                var user = await _userRepository.GetUserByIdAsync(id);
                return user != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if user exists with id: {Id}", id);
                throw;
            }
        }  
      

        public async Task<IList<string>> GetUserRolesAsync(User user)
        {
            try
            {
                return await _userManager.GetRolesAsync(user);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching roles for user");
                throw;
            }
        }

      
        }
    }

