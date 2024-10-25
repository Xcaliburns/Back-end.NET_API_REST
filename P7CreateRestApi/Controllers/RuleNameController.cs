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

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RuleNameController : ControllerBase
    {
        private readonly IRuleNameServices _ruleNameServices;
        public RuleNameController(IRuleNameServices ruleNameServices)
        {
            _ruleNameServices = ruleNameServices;
        }
        // GET: api/RuleName
        [HttpGet]
        public async Task<ActionResult<IEnumerable<RuleName>>> GetRuleName()
        {
            var ruleNames = await _ruleNameServices.GetAllRatingsAsync();
            return Ok(ruleNames);
        }
        // GET: api/RuleName/5
        [HttpGet("{id}")]
        public async Task<ActionResult<RuleName>> GetRuleName(int id)
        {
            var ruleName = await _ruleNameServices.GetRuleByIdAsync(id);
            if (ruleName == null)
            {
                return NotFound();
            }
            return Ok(ruleName);
        }
        //PUT :api/RuleName/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRuleName(int id, RuleName ruleName)
        {
            try
            {
                await _ruleNameServices.UpdateRuleAsync(id, ruleName);
            }
            catch (ArgumentException)
            {
                return BadRequest();
            }
            catch (Exception)
            {
                return StatusCode(StatusCodes.Status500InternalServerError);
            }
            return NoContent();
        }
        // POST: api/RuleName
        [HttpPost]
        public async Task<ActionResult<RuleName>> PostRuleName(RuleName ruleName)
        {
            await _ruleNameServices.AddRuleAsync(ruleName);
            return CreatedAtAction("GetRuleName", new { id = ruleName.Id }, ruleName);
        }
        // DELETE: api/RuleName/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRuleName(int id)
        {
            await _ruleNameServices.DeleteRuleAsync(id);
            return NoContent();
        }

    }


}

