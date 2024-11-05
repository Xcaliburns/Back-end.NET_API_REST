using Findexium.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Findexium.Infrastructure.Models
{//TODO revoir cette classe avec Laala
    internal class UsersDto
    {
        public int Id { get; internal set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get;set; }
        public string Role { get; set; }
       

        public UsersDto(string userName, string password, string fullName, string role)
        {
            UserName = userName;
            Password = password;
            FullName = fullName;
            Role = role;
           
        }

        internal User ToUser()
        {
            return new User
            {
                UserName = UserName,
                PasswordHash = Password,// Je ne sais plus si ce n'est pas l'inverse
                Fullname = FullName,
                Role = Role,
               
            };
        }
    }
}
