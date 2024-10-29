using Microsoft.AspNetCore.Identity;

namespace Findexium.Domain.Models
{
    public class User : IdentityUser
    {
        // Propriétés supplémentaires
        public string Fullname { get; set; } = string.Empty;
        // Vous n'avez pas besoin de redéfinir UserName et Password ici
        // public string Role { get; set; } // Les rôles sont gérés séparément
    }
}