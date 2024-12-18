using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class LoginRequest
    {

        [Required(ErrorMessage = "Nom d'utilisateur requis.")]
        [Display(Name = "Utilisateur")]
        public string Login { get; set; }

        [Required(ErrorMessage = "mot de passe requis.")]
        [Display(Name = "Mot de passe")]       
        public string Password { get; set; }
    }
}
