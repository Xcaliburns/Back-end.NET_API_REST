using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
   
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userService.GetUsersAsync();
                var userDto = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.Fullname,
                }).ToList();
                return Ok(userDto);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<UserResponse>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation("Fetching user with id: {Id}", id);
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }

                var userResponse = new UserResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    FullName = user.Fullname
                };

                return Ok(userResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching user with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> PutUser(string id, UserRequest userRequest)
        {
            try
            {
                _logger.LogInformation("Updating user with id: {Id}", id);
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    return NotFound();
                }

                // Mettre à jour les propriétés
                existingUser.UserName = userRequest.UserName;
                existingUser.Fullname = userRequest.FullName;

                // Mettre à jour le mot de passe si fourni
                if (!string.IsNullOrEmpty(userRequest.Password))
                {
                    var passwordHasher = new PasswordHasher<User>();
                    existingUser.PasswordHash = passwordHasher.HashPassword(existingUser, userRequest.Password);
                }

                var result = await _userService.UpdateUserAsync(existingUser);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
                }
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating user with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPost]
        public async Task<ActionResult<UserResponse>> PostUser(UserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                var user = request.ToUser();
                _logger.LogInformation("Creating a new user");
                //verifie que le userName est unique
                var result = await _userService.AddUserAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var userResponse = new UserResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        FullName = user.Fullname
                    };
                    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, userResponse);
                }

                return BadRequest(result.Errors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new user");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation("Deleting user with id: {Id}", id);

                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    return NotFound();
                }

                var result = await _userService.DeleteUserAsync(id);
                if (!result.Succeeded)
                {
                    return NotFound();
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting user with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}