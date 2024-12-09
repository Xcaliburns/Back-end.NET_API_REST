using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;

namespace Findexium.Infrastructure.Models
{//TODO revoir cette classe avec Laala
    public class UsersDto
    {
        public int Id { get; internal set; }
        public string UserName { get; set; }
        public string PasswordHash { get; set; }
        public string FullName { get; set; }
       

        public UsersDto(
            string userName,
            string password,
            string fullName)
          
        {
            UserName = userName;
            PasswordHash = password;
            FullName = fullName;
           
        }

        internal User ToUser(UserManager<User> userManager)
        {
            var user = new User
            {
                UserName = UserName,
                Fullname = FullName,
               
            };

            // Hash the password using UserManager to ensure it is always secured
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, PasswordHash);

            return user;
        }
    }
}
