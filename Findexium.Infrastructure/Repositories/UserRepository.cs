using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Findexium.Infrastructure.Models;

namespace Findexium.Infrastructure.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly LocalDbContext _context;
        private readonly UserManager<User> _userManager;

        public UserRepository(LocalDbContext context, UserManager<User> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        public async Task<IEnumerable<User>> GetUsersAsync()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _context.Users.FindAsync(id);
        }

        public async Task AddUserAsync(User user)
        {
            var userDto = new UsersDto(user.UserName, user.PasswordHash, user.Fullname, user.Role);
            var newUser = userDto.ToUser(_userManager);

            var result = await _userManager.CreateAsync(newUser);
            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(newUser, newUser.Role);
            }
            else
            {
                throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
            }
        }

        public async Task UpdateUserAsync(User user)
        {
            var existingUser = await _context.Users.FindAsync(user.Id);
            if (existingUser != null)
            {
                existingUser.UserName = user.UserName;
                existingUser.Fullname = user.Fullname;
                existingUser.Role = user.Role;

                var result = await _userManager.UpdateAsync(existingUser);
                if (result.Succeeded)
                {
                    var currentRoles = await _userManager.GetRolesAsync(existingUser);
                    await _userManager.RemoveFromRolesAsync(existingUser, currentRoles);
                    await _userManager.AddToRoleAsync(existingUser, user.Role);
                }
                else
                {
                    throw new Exception(string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }
        }

        public async Task DeleteUserAsync(string id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user != null)
            {
                _context.Users.Remove(user);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> UserExistsAsync(string id)
        {
            return await _context.Users.AnyAsync(e => e.Id == id);
        }
    }
}