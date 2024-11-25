namespace Findexium.Api.Models
{
    public class LoginRequest
    {
        //TODO: verifier s'il faut le modifier (login , mail, username ...)
        //TODO : peut etre que le Login devait etre le userName au lieu du mail

        public string Login { get; set; }
        public string Password { get; set; }
    }
}
