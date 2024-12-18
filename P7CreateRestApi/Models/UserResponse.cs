using System.ComponentModel.DataAnnotations;

namespace Findexium.Api.Models
{
    public class UserResponse
    {

        public string Id { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
    }
}
