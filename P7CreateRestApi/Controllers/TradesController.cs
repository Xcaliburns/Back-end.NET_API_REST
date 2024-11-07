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
{ //TODO : revoir ce controller , il utilise des vues et non des api
    [Route("api/[controller]")]
    [ApiController]
    public class TradesController : ControllerBase
    {
        private readonly ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // GET: api/Trades
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Trade>>> GetTrades()
        {
            var trades = await _tradeService.GetAllTradesAsync();
            return Ok(trades);
        }

        // GET: api/Trades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Trade>> GetTrade(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);

            if (trade == null)
            {
                return NotFound();
            }

            return Ok(trade);
        }

        // POST: api/Trades
        [HttpPost]
        public async Task<ActionResult<Trade>> CreateTrade(TradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var trade = request.ToTrade();
            await _tradeService.AddTradeAsync(trade);

            return CreatedAtAction(nameof(GetTrade), new { id = trade.TradeId }, trade);
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
                await _tradeService.UpdateTradeAsync(trade);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _tradeService.TradeExistsAsync(id))
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

        // DELETE: api/Trades/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteTrade(int id)
        {
            var trade = await _tradeService.GetTradeByIdAsync(id);
            if (trade == null)
            {
                return NotFound();
            }

            await _tradeService.DeleteTradeAsync(id);

            return NoContent();
        }
    }
}
