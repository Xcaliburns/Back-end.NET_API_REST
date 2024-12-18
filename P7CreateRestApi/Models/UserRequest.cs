using Findexium.Domain.Models;
using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class UserRequest
    {
        
        [Required]
        [StringLength(50, ErrorMessage = "Username cannot be longer than 50 characters.")]
        public string UserName { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Password ne peut pas etre plus long que 100 caracteres.")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Le nom complet ne peut pas etre plus long que 100 caracteres.")]
        public string FullName { get; set; }

        public User ToUser()
        {
            return new User
            {
                UserName = UserName,
                Fullname = FullName
            };
        }
    }
}
