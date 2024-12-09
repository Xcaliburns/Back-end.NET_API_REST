using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Models
{
    public class User : IdentityUser
    {
        
       
        public string Fullname { get; set; }
      

       
    }
}