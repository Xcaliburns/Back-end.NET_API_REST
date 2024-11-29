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
using Microsoft.AspNetCore.Authorization;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
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
        public async Task<ActionResult<IEnumerable<RuleNameResponse>>> GetRuleName()
        {
            try
            {
                _logger.LogInformation("Fetching all rule names");
                var ruleNames = await _ruleNameServices.GetAllRulesAsync();
                var ruleNameDtos = ruleNames.Select(r => new RuleNameResponse
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    Json = r.Json,
                    Template = r.Template,
                    SqlStr = r.SqlStr,
                    SqlPart = r.SqlPart,
                }).ToList();
                return Ok(ruleNameDtos);
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

            // TODO : verifier si le model est valide
            if (id != ruleName.Id)
            {
                _logger.LogWarning("ID mismatch for rule name with id: {Id}", id);
                return BadRequest();
            }
            try
            {
                _logger.LogInformation("Updating rule name with id: {Id}", id);
                await _ruleNameServices.UpdateRuleAsync(ruleName);
            }
            catch (ArgumentException ex)
            {
                _logger.LogWarning(ex, "Invalid argument for rule name with id: {Id}", id);
                return BadRequest(ex.Message);
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
        public async Task<IActionResult> PostRuleName(RuleNameRequest request)
        {
            try
            {
                if(!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for ruleNameRequest");
                    return BadRequest(ModelState);
                }
                _logger.LogInformation("Creating a new rule name");
                var ruleName = request.ToRuleName();
                await _ruleNameServices.AddRuleAsync(ruleName);

                return Created();
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
                var ruleName = await _ruleNameServices.GetRuleByIdAsync(id);
                if (ruleName == null)
                {
                    _logger.LogWarning("Rule name with id: {Id} not found", id);
                    return NotFound();
                }

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

