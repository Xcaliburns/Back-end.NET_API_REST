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
using Findexium.Domain.Services;


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
        public async Task<ActionResult<IEnumerable<RatingResponse>>> GetRatings()
        {
            try
            {
                _logger.LogInformation("Getting all ratings");
                var ratings = await _ratingService.GetAllRatingsAsync();
                var ratingDtos = ratings.Select(r => new RatingResponse
                {
                    Id = r.Id,
                    MoodysRating = r.MoodysRating,
                    SandPRating = r.SandPRating,
                    FitchRating = r.FitchRating,
                    OrderNumber = r.OrderNumber
                }).ToList();
                return Ok(ratingDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all ratings");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
        // GET: api/Ratings/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingResponse>> GetRating(int id)
        {
            try
            {
                _logger.LogInformation("Fetching rating with id: {Id}", id);
                var rating = await _ratingService.GetRatingByIdAsync(id);
                if (rating == null)
                {
                    _logger.LogWarning("Rating with id: {Id} not found", id);
                    return NotFound();
                }

                var ratingResponse = new RatingResponse
                {
                    Id = rating.Id,
                    MoodysRating = rating.MoodysRating,
                    SandPRating = rating.SandPRating,
                    FitchRating = rating.FitchRating,
                    OrderNumber = rating.OrderNumber
                };

                return Ok(ratingResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching rating with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutRating(int id, RatingRequest ratingRequest)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for RatingRequest");
                return BadRequest(ModelState);
            }

          
            try
            {
                _logger.LogInformation("Updating rating with id {Id}", id);
                var existingRating = await _ratingService.GetRatingByIdAsync(id);
                if (existingRating == null)
                {
                    _logger.LogWarning("Rating with id {Id} not found", id);
                    return NotFound($"Rating with id {id} not found.");
                }

                var updatedRating = ratingRequest.ToRating();
                updatedRating.Id = id; // Ensure the ID remains the same

                await _ratingService.UpdateRatingAsync(updatedRating);
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
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Ratings
        [HttpPost]
        public async Task<IActionResult> PostRating(RatingRequest request)
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

               return Created();
               
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
