namespace Dot.Net.WebApi.Domain
{
    public class User //: IdentityUser
    {
        //TODO : voir avec Laala : les champs de la table USER sont ils pour la plupart d�ja inclus dans IdentityUser ?
        public string UserName { get; set; }
    }
}