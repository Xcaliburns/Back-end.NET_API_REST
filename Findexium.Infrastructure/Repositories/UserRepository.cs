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
        private readonly ILogger<UserRepository> _logger;

        public UserRepository(LocalDbContext context, UserManager<User> userManager, ILogger<UserRepository> logger)
        {
            _context = context;
            _userManager = userManager;
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
                throw;
            }
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            try
            {
                return await _context.Users.FindAsync(id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user by id: {Id}", id);
                throw;
            }
        }

        public async Task AddUserAsync(User user)
        {
            try
            {
                _context.Users.Add(user);
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while adding a new user");
                throw;
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.Id);
                if (existingUser != null)
                {
                    existingUser.UserName = user.UserName;
                    existingUser.Fullname = user.Fullname;

                    var result = await _userManager.UpdateAsync(existingUser);
                    if (result.Succeeded)
                    {
                        var currentRoles = await _userManager.GetRolesAsync(existingUser);
                        await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    }
                    else
                    {
                        throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user");
                throw;
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            try
            {
                var user = await _context.Users.FindAsync(id);
                if (user != null)
                {
                    _context.Users.Remove(user);
                    await _context.SaveChangesAsync();
                }
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
                return await _context.Users.AnyAsync(e => e.Id == id);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while checking if user exists with id: {Id}", id);
                throw;
            }
        }
    }
}