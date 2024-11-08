using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Api.Models;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
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
        public async Task<ActionResult<IEnumerable<CurvePoint>>> GetCurvePoints()
        {
            try
            {
                _logger.LogInformation("Fetching all curve points");
                var curvePoints = await _curvePointService.GetAllAsync();
                return Ok(curvePoints);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all curve points");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/CurvePoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePoint>> GetCurvePoint(int id)
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

                return Ok(curvePoint);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching curve point with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/CurvePoints/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurvePoint(int id, CurvePoint curvePoint)
        {
            if (id != curvePoint.Id)
            {
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Updating curve point with id: {Id}", id);
                await _curvePointService.UpdateAsync(id, curvePoint);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _curvePointService.GetByIdAsync(id) == null)
                {
                    _logger.LogWarning("Curve point with id: {Id} not found during update", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating curve point with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/CurvePoints
        [HttpPost]
        public async Task<ActionResult<CurvePoint>> PostCurvePoint(CurvePointRequest request)
        {
            try
            {
                _logger.LogInformation("Creating a new curve point");
                var curvePoint = request.ToCurvePoint();
                await _curvePointService.AddAsync(curvePoint);
                return CreatedAtAction(nameof(GetCurvePoint), new { id = curvePoint.Id }, curvePoint);
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
