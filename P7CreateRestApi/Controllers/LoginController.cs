using Findexium.Api.Models;
using Microsoft.AspNetCore.Mvc;

namespace Findexium.Api.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class LoginController : ControllerBase
    {
        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            //TODO: implement the UserManager from Identity to validate User and return a security token.
            return Unauthorized();
        }
    }
}