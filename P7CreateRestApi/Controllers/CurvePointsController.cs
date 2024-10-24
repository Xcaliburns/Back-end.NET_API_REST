using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using Findexium.Domain.Interfaces;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CurvePointsController : ControllerBase
    {
        private readonly ICurvePointServices _CurvePointService;

        public CurvePointsController(ICurvePointServices curvePointService)
        {
            _CurvePointService = curvePointService;
        }

        // GET: api/CurvePoints
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CurvePoint>>> GetCurvePoints()
        {
            var curvePoints = await _CurvePointService.GetAllAsync();
            return Ok(curvePoints);
        }

        // GET: api/CurvePoints/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CurvePoint>> GetCurvePoint(int id)
        {
            var curvePoint = await _CurvePointService.GetByIdAsync(id);

            if (curvePoint == null)
            {
                return NotFound();
            }

            return Ok(curvePoint);
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
                await _CurvePointService.UpdateAsync(id, curvePoint);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (await _CurvePointService.GetByIdAsync(id) == null)
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/CurvePoints
        [HttpPost]
        public async Task<ActionResult<CurvePoint>> PostCurvePoint(CurvePoint curvePoint)
        {
            await _CurvePointService.AddAsync(curvePoint);
            return CreatedAtAction("GetCurvePoint", new { id = curvePoint.Id }, curvePoint);
        }

        // DELETE: api/CurvePoints/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurvePoint(int id)
        {
            var curvePoint = await _CurvePointService.GetByIdAsync(id);
            if (curvePoint == null)
            {
                return NotFound();
            }

            await _CurvePointService.DeleteAsync(id);
            return NoContent();
        }
    }
}
