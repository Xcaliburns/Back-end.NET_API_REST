using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Models
{//TODO revoir cette classe avec Laala
    public class UsersDto
    {
        public int Id { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }
        [JsonIgnore] // Ignore this property during serialization/deserialization
        public string Role { get; set; } = "User"; // Default role is "User"

        public UsersDto(
            string userName,
            string password,
            string fullName,
            string role
        )
        {
            UserName = userName;
            Password = password;
            FullName = fullName;
            Role = role;
        }

        internal User ToUser(UserManager<User> userManager)
        {
            var user = new User
            {
                UserName = UserName,
                Fullname = FullName,
                Role = Role
            };

            // Hash the password using UserManager to ensure it is always secured
            user.PasswordHash = userManager.PasswordHasher.HashPassword(user, Password);

            return user;
        }
    }
}
