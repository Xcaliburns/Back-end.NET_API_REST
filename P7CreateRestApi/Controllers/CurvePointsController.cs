﻿using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
    public class CurvePointsController : ControllerBase
    {
        private readonly ICurvePointServices _curvePointService;
        private readonly ILogger<CurvePointsController> _logger;

        public CurvePointsController(ICurvePointServices curvePointService, ILogger<CurvePointsController> logger)
        {
            _curvePointService = curvePointService;
            _logger = logger;
        }

        // GET: api/CurvePoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePointResponse>>> GetCurvePoints()
        {
            try
            {
                _logger.LogInformation("Fetching all curve points");
                var curvePoints = await _curvePointService.GetAllAsync();
                var curvePointDtos = curvePoints.Select(c => new CurvePointResponse
                {
                    Id = c.Id,
                    CurveId = c.CurveId,                 
                    Term = c.Term,
                    CurvePointValue = c.CurvePointValue,
                  
                }).ToList();
                return Ok(curvePointDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all curve points");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/CurvePoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePointResponse>> GetCurvePoint(int id)
        {
            try
            {
                _logger.LogInformation("Fetching curve point with id: {Id}", id);
                var curvePoint = await _curvePointService.GetByIdAsync(id);

                if (curvePoint == null)
                {
                    _logger.LogWarning("Curve point with id: {Id} not found", id);
                    return NotFound();
                }

                var curvePointResponse = new CurvePointResponse
                {
                    Id = curvePoint.Id,
                    CurveId = curvePoint.CurveId,
                    Term = curvePoint.Term,
                    CurvePointValue = curvePoint.CurvePointValue
                };

                return Ok(curvePointResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching curve point with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/CurvePoints/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurvePoint(int id, CurvePointRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for CurvePointRequest");
                return BadRequest(ModelState);
            }           

            try
            {
                _logger.LogInformation("Updating curve point with id: {Id}", id);
                var existingCurvePoint = await _curvePointService.GetByIdAsync(id);
                if (existingCurvePoint == null)
                {
                    _logger.LogWarning("Curve point with id: {Id} not found", id);
                    return NotFound();
                }

                var updatedCurvePoint = request.ToCurvePoint();
                updatedCurvePoint.Id = id; // Ensure the ID remains the same

                await _curvePointService.UpdateAsync(updatedCurvePoint);
                return NoContent();
            }
           
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating curve point with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/CurvePoints
        [HttpPost]
        public async Task<IActionResult> PostCurvePoint(CurvePointRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for CurvePointRequest");
                    return BadRequest(ModelState);
                }

                _logger.LogInformation("Creating a new curve point");
                var curvePoint = request.ToCurvePoint();
                await _curvePointService.AddAsync(curvePoint);

                return Created();
                
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new curve point");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/CurvePoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            try
            {
                _logger.LogInformation("Deleting curve point with id: {Id}", id);
                var curvePoint = await _curvePointService.GetByIdAsync(id);
                if (curvePoint == null)
                {
                    _logger.LogWarning("Curve point with id: {Id} not found during deletion", id);
                    return NotFound();
                }

                await _curvePointService.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting curve point with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
