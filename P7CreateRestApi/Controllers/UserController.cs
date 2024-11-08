using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Infrastructure.Models;
using Findexium.Infrastructure.Repositories;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Findexium.Api.Controllers
{

    //TODO : voir avec Laala pour la séparation DDD
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserManager<User> _userManager;
        private readonly ILogger<UsersController> _logger;

        public UsersController(UserManager<User> userManager, ILogger<UsersController> logger)
        {
            _userManager = userManager;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            try
            {
                _logger.LogInformation("Fetching all users");
                var users = await _userManager.Users.ToListAsync();
                return Ok(users);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all users");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(string id)
        {
            try
            {
                _logger.LogInformation("Fetching user with id: {Id}", id);
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }
                return Ok(user);
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
                var existingUser = await _userManager.FindByIdAsync(id);
                if (existingUser == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }

                existingUser.UserName = user.UserName;
                existingUser.Email = user.Email;
                // Update other properties as needed

                var result = await _userManager.UpdateAsync(existingUser);
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
        public async Task<ActionResult<User>> PostUser(UserRequest request)
        {
            if (request == null)
            {
                return BadRequest("Invalid user data.");
            }

            try
            {
                _logger.LogInformation("Creating a new user");
                var user = request.ToUser();
                var result = await _userManager.CreateAsync(user, request.Password);
                if (result.Succeeded)
                {
                    return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
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
                var user = await _userManager.FindByIdAsync(id);
                if (user == null)
                {
                    _logger.LogWarning("User with id: {Id} not found", id);
                    return NotFound();
                }

                var result = await _userManager.DeleteAsync(user);
                if (!result.Succeeded)
                {
                    return BadRequest(result.Errors);
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