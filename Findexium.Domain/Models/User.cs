namespace Dot.Net.WebApi.Domain
{
    public class User //: IdentityUser
    {
        //TODO : voir avec Laala : les champs de la table USER sont ils pour la plupart déja inclus dans IdentityUser ?

        public int Id {get; set; }
        public int UserName { get; set; }
        public int Password { get; set; }
        public int Fullname { get; set; }
        public string Role { get; set; }
    }
}