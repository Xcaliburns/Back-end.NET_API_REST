namespace Findexium.Api.Models
{
    public class LoginRequest
    {
        //TODO: verifier s'il faut le modifier (login , mail, username ...)
        public string Login { get; set; }
        public string Password { get; set; }
    }
}
