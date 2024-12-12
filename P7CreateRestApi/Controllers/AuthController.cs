using Findexium.Api.Models;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Findexium.Api.Controllers
{

    
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<AuthController> _logger;

        public AuthController(IConfiguration configuration, UserManager<User> userManager, ILogger<AuthController> logger)
        {
            _configuration = configuration;
            _userManager = userManager;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest loginRequest)
        {
            try
            {
                _logger.LogInformation("Attempting to log in user with login: {Login}", loginRequest.Login);

                // Validation des informations d'identification directement dans le contrôleur
                var user = await _userManager.FindByNameAsync(loginRequest.Login);
                if (user != null && await _userManager.CheckPasswordAsync(user, loginRequest.Password))
                {
                    var userRoles = await _userManager.GetRolesAsync(user);
                    var tokenHandler = new JwtSecurityTokenHandler();
                    var key = Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]);
                    var tokenDescriptor = new SecurityTokenDescriptor
                    {
                        Subject = new ClaimsIdentity(new[]
                        {
                                new Claim(ClaimTypes.Name, user.UserName),
                                new Claim(ClaimTypes.Role, userRoles.FirstOrDefault() ?? "User") // Assuming a single role for simplicity
                            }),
                        Expires = DateTime.UtcNow.AddHours(1),
                        SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
                        Issuer = _configuration["Jwt:Issuer"],
                        Audience = _configuration["Jwt:Audience"]
                    };
                    var token = tokenHandler.CreateToken(tokenDescriptor);
                    var tokenString = "Bearer " + tokenHandler.WriteToken(token);

                    _logger.LogInformation("User {Login} logged in successfully", loginRequest.Login);
                    return Ok(new { Token = tokenString });
                }

                _logger.LogWarning("Invalid login attempt for user: {Login}", loginRequest.Login);
                return Unauthorized();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while logging in user: {Login}", loginRequest.Login);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
