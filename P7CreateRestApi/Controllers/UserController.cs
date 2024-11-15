using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Domain.Services;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Api.Controllers
{


    [Route("api/[controller]")]
    [Authorize(Roles = "Admin")]
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
        public async Task<ActionResult<IEnumerable<UserResponse>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userService.GetUsersAsync();
                var userResponses = users.Select(user => new UserResponse
                {
                    Id = user.Id,
                    UserName = user.UserName,
                    Password = user.PasswordHash, // Assuming PasswordHash is used for Password
                    FullName = user.Fullname,
                    Role = user.Role
                }).ToList();
                return Ok(userResponses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
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
                    Password = user.PasswordHash, // Assuming PasswordHash is used for Password
                    FullName = user.Fullname,
                    Role = user.Role
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
        public async Task<IActionResult> PutUser(string id, User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Updating user with id: {Id}", id);
                var existingUser = await _userService.GetUserByIdAsync(id);
                if (existingUser == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                // Update other properties as needed

                await _userService.UpdateUserAsync(existingUser);
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
                _logger.LogInformation("Creating a new user");
                var user = request.ToUser();
                var result = await _userService.AddUserAsync(user, request.Password);
                if (result.Succeeded)
                {
                    var userResponse = new UserResponse
                    {
                        Id = user.Id,
                        UserName = user.UserName,
                        Password = user.PasswordHash, // Assuming PasswordHash is used for Password
                        FullName = user.Fullname,
                        Role = user.Role
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
        public async Task<IActionResult> DeleteUser(string id)
        {
            try
            {
                _logger.LogInformation("Deleting user with id: {Id}", id);
                var user = await _userService.GetUserByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }

                await _userService.DeleteUserAsync(id);
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