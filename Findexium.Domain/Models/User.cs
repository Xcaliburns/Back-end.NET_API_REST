using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Models
{
    public class User : IdentityUser
    {
        // Propri�t�s suppl�mentaires

     //   public string userName { get; set; } = string.Empty;
     
      //  public string Password { get; set; }
       
        public string Fullname { get; set; }
        public string Role { get; set; } = "User"; //attribue le role user par d�faut

        // Vous n'avez pas besoin de red�finir UserName et Password ici
        // public string Role { get; set; } // Les r�les sont g�r�s s�par�ment
    }
}