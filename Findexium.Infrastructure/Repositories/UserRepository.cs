using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Findexium.Infrastructure.Models;
using Microsoft.Extensions.Logging;

namespace Findexium.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _context;
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(LocalDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, ILogger<UserRepository> logger)
        {
            _context = context;
            _userManager = userManager;
            _roleManager = roleManager;
            _logger = logger;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            try
            {
                return await _context.Users.ToListAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching users");
                throw new Exception("An error occurred while fetching users.", ex);
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                return user ?? throw new Exception($"User with id: {id} not found.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by id: {Id}", id);
                throw new Exception($"An error occurred while fetching user by id.", ex);
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                
                if (!await _roleManager.RoleExistsAsync("User"))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole("User"));
                    if (!roleResult.Succeeded)
                    {
                        throw new Exception(string.Join(", ", roleResult.Errors.Select(e => e.Description)));
                    }
                }

                
                var password = user.PasswordHash ?? throw new ArgumentNullException(nameof(user.PasswordHash), "Password cannot be null.");

                
                var result = await _userManager.CreateAsync(user, password);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }

                
                var roleAddResult = await AddUserToRoleAssync(user, "User");
                if (!roleAddResult.Succeeded)
                {
                    _logger.LogError("Failed to add role to user: {Errors}", string.Join(", ", roleAddResult.Errors.Select(e => e.Description)));
                    throw new Exception(string.Join(", ", roleAddResult.Errors.Select(e => e.Description)));
                }

                _logger.LogInformation("Role 'User' successfully added to user {UserId}", user.Id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new user");
                throw new Exception("An error occurred while adding the user.", ex);
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user), "User cannot be null.");
            }

            if (string.IsNullOrEmpty(user.Id))
            {
                throw new ArgumentException("User ID cannot be null or empty.", nameof(user.Id));
            }

            try
            {
                var existingUser = await _userManager.FindByIdAsync(user.Id);
                if (existingUser == null)
                {
                    throw new Exception("User not found.");
                }

                existingUser.UserName = user.UserName ?? existingUser.UserName;
                existingUser.Fullname = user.Fullname ?? existingUser.Fullname;

                
                if (!string.IsNullOrEmpty(user.PasswordHash))
                {
                    existingUser.PasswordHash = _userManager.PasswordHasher.HashPassword(existingUser, user.PasswordHash);
                }

                var result = await _userManager.UpdateAsync(existingUser);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw new Exception("An error occurred while updating the user.", ex);
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            try
            {
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    throw new Exception("User not found.");
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with id: {Id}", id);
                throw new Exception($"An error occurred while deleting user with id: {id}", ex);
            }
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            try
            {
                return await _userManager.FindByIdAsync(id) != null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if user exists with id: {Id}", id);
                throw new Exception($"An error occurred while checking if user exists with id: {id}", ex);
            }
        }

        public async Task<IdentityResult> AddUserToRoleAssync(User user, string roleName)
        {
            try
            {
                
                if (!await _roleManager.RoleExistsAsync(roleName))
                {
                    var roleResult = await _roleManager.CreateAsync(new IdentityRole(roleName));
                    if (!roleResult.Succeeded)
                    {
                        return roleResult;
                    }
                }

                // Add the role to the user
                _logger.LogInformation("Adding role {RoleName} to user {UserId}", roleName, user.Id);
                return await _userManager.AddToRoleAsync(user, roleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding role to user");
                return IdentityResult.Failed(new IdentityError { Description = "An error occurred while adding the role to the user." });
            }
        }
    }
}