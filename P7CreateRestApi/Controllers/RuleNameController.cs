using System;
using System.Collections.Generic;
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
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameServices _ruleNameServices;
        private readonly ILogger<RuleNameController> _logger;

        public RuleNameController(IRuleNameServices ruleNameServices, ILogger<RuleNameController> logger)
        {
            _ruleNameServices = ruleNameServices;
            _logger = logger;
        }

        // GET: api/RuleName
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleName>>> GetRuleName()
        {
            try
            {
                _logger.LogInformation("Fetching all rule names");
                var ruleNames = await _ruleNameServices.GetAllRatingsAsync();
                return Ok(ruleNames);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all rule names");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/RuleName/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RuleName>> GetRuleName(int id)
        {
            try
            {
                _logger.LogInformation("Fetching rule name with id: {Id}", id);
                var ruleName = await _ruleNameServices.GetRuleByIdAsync(id);

                if (ruleName == null)
                {
                    _logger.LogWarning("Rule name with id: {Id} not found", id);
                    return NotFound();
                }

                return Ok(ruleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching rule name with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/RuleName/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuleName(int id, RuleName ruleName)
        {
            try
            {
                _logger.LogInformation("Updating rule name with id: {Id}", id);
                await _ruleNameServices.UpdateRuleAsync(id, ruleName);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for rule name with id: {Id}", id);
                return BadRequest();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating rule name with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/RuleName
        [HttpPost]
        public async Task<ActionResult<RuleName>> PostRuleName(RuleNameRequest request)
        {
            try
            {
                _logger.LogInformation("Creating a new rule name");
                var ruleName = request.ToRuleName();
                await _ruleNameServices.AddRuleAsync(ruleName);
                return CreatedAtAction(nameof(GetRuleName), new { id = ruleName.Id }, ruleName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new rule name");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // DELETE: api/RuleName/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            try
            {
                _logger.LogInformation("Deleting rule name with id: {Id}", id);
                await _ruleNameServices.DeleteRuleAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting rule name with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }


}

