using Findexium.Domain.Models;

namespace Findexium.Api.Models
{
    public class UserRequest
    {

        public string UserName { get; set; }
        public string Password { get; set; }
        public string FullName { get; set; }


        internal User ToUser()
        {
            return new User
            {
                UserName = UserName,
            //    Password = Password,
                Fullname = FullName

            };
        }
    }
}
