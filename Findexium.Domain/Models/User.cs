using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Models
{
    public class User : IdentityUser
    {
        // Propriétés supplémentaires

     //   public string userName { get; set; } = string.Empty;
     
      //  public string Password { get; set; }
       
        public string Fullname { get; set; }
        public string Role { get; set; } = "User"; //attribue le role user par défaut

        // Vous n'avez pas besoin de redéfinir UserName et Password ici
        // public string Role { get; set; } // Les rôles sont gérés séparément
    }
}