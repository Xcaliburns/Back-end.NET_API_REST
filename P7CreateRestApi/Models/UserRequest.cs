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
        [StringLength(100, ErrorMessage = "Password cannot be longer than 100 characters.")]
        public string Password { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "Full name cannot be longer than 100 characters.")]
        public string FullName { get; set; }

        internal User ToUser()
        {
            return new User
            {
                UserName = UserName,
                Fullname = FullName
            };
        }
    }
}
