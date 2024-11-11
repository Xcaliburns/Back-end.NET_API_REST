using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

using Findexium.Domain.Interfaces;
using Findexium.Domain.Services;
using Findexium.Domain.Models;
using Findexium.Api.Models;

namespace Findexium.Api.Controllers
{ 
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService _tradeService;
        private readonly ILogger<TradesController> _logger;

        public TradesController(ITradeService tradeService, ILogger<TradesController> logger)
        {
            _tradeService = tradeService;
            _logger = logger;
        }

        // GET: api/Trades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetTrades()
        {
            try
            {
                _logger.LogInformation("Fetching all trades");
                var trades = await _tradeService.GetAllTradesAsync();
                return Ok(trades);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all trades");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Trades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTrade(int id)
        {
            try
            {
                _logger.LogInformation("Fetching trade with id: {Id}", id);
                var trade = await _tradeService.GetTradeByIdAsync(id);

                if (trade == null)
                {
                    _logger.LogWarning("Trade with id: {Id} not found", id);
                    return NotFound();
                }

                return Ok(trade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching trade with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Trades
        [HttpPost]
        public async Task<ActionResult<Trade>> CreateTrade(TradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating a new trade");
                var trade = request.ToTrade();
                await _tradeService.AddTradeAsync(trade);
                return CreatedAtAction(nameof(GetTrade), new { id = trade.TradeId }, trade);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new trade");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/Trades/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateTrade(int id, Trade trade)
        {
            if (id != trade.TradeId)
            {
                return BadRequest();
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Updating trade with id: {Id}", id);
                await _tradeService.UpdateTradeAsync(trade);
            }
            catch (DbUpdateConcurrencyException ex)
            {
                if (!await _tradeService.TradeExistsAsync(id))
                {
                    _logger.LogWarning("Trade with id: {Id} not found", id);
                    return NotFound();
                }
                else
                {
                    _logger.LogError(ex, "Concurrency error occurred while updating trade with id: {Id}", id);
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating trade with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // DELETE: api/Trades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            try
            {
                _logger.LogInformation("Deleting trade with id: {Id}", id);
                var trade = await _tradeService.GetTradeByIdAsync(id);
                if (trade == null)
                {
                    _logger.LogWarning("Trade with id: {Id} not found", id);
                    return NotFound();
                }

                await _tradeService.DeleteTradeAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting trade with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
