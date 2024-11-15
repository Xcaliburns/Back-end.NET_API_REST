using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Api.Models;
using Microsoft.AspNetCore.Authorization;


namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    public class RatingsController : ControllerBase
    {
        private readonly IRatingServices _ratingService;
        private readonly ILogger<RatingsController> _logger;

        public RatingsController(IRatingServices ratingService, ILogger<RatingsController> logger)
        {
            _ratingService = ratingService;
            _logger = logger;
        }

        // GET: api/Ratings
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Rating>>> GetRating()
        {
            try
            {
                _logger.LogInformation("Getting all ratings");
                var ratings = await _ratingService.GetAllRatingsAsync();
                return Ok(ratings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ratings");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Rating>> GetRating(int id)
        {
            try
            {
                _logger.LogInformation("Getting rating with id {Id}", id);
                var rating = await _ratingService.GetRatingByIdAsync(id);

                if (rating == null)
                {
                    _logger.LogWarning("Rating with id {Id} not found", id);
                    return NotFound();
                }

                return Ok(rating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting rating with id {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Ratings/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, Rating rating)
        {
            try
            {
                _logger.LogInformation("Updating rating with id {Id}", id);
                await _ratingService.UpdateRatingAsync(id, rating);
                return NoContent();
            }
            catch (ArgumentException ex)
            {
                _logger.LogError(ex, "Invalid argument for rating with id {Id}", id);
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating rating with id {Id}", id);
                if (await _ratingService.GetRatingByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
                }
            }
        }

        // POST: api/Ratings
        [HttpPost]
        public async Task<ActionResult<RatingRequest>> PostRating(RatingRequest request)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogWarning("invalid model state");
                    return BadRequest(ModelState);
                }
                var rating = request.ToRating();
                _logger.LogInformation("Creating a new rating");
                await _ratingService.AddRatingAsync(rating);

                var createdRating= new RatingRequest
                {
                   
                    MoodysRating = rating.MoodysRating,
                    SandPrating = rating.SandPRating,
                    FitchRating = rating.FitchRating,
                    OrderNumber = rating.OrderNumber
                };



                return CreatedAtAction(nameof(GetRating), new { id = rating.Id }, createdRating);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating a new rating");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/Ratings/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRating(int id)
        {
            try
            {
                _logger.LogInformation("Deleting rating with id {Id}", id);
                var rating = await _ratingService.GetRatingByIdAsync(id);
                if (rating == null)
                {
                    _logger.LogWarning("Rating with id {Id} not found", id);
                    return NotFound();
                }

                await _ratingService.DeleteRatingAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting rating with id {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
