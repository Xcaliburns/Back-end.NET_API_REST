using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Findexium.Infrastructure.Models
{
    public class UsersDto
    {
        public int Id { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }

        public UsersDto(string userName, string password, string fullName)
        {
            UserName = userName;
            Password = password;
            FullName = fullName;
        }

        public User ToUser(UserManager<User> userManager)
        {
            var user = new User
            {
                UserName = UserName,
                Fullname = FullName,
            };


            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, Password);

            return user;
        }
    }
}
