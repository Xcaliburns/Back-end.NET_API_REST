using Findexium.Api.Models;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
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
        public async Task<ActionResult<IEnumerable<TradeResponse>>> GetTrades()
        {
            try
            {
                _logger.LogInformation("Fetching all trades");
                var trades = await _tradeService.GetAllTradesAsync();
                var tradeDtos = trades.Select(t => new TradeResponse
                {
                    TradeId = t.TradeId,
                    Account = t.Account,
                    AccountType = t.AccountType,
                    BuyQuantity = t.BuyQuantity,
                    SellQuantity = t.SellQuantity,
                    BuyPrice = t.BuyPrice,
                    SellPrice = t.SellPrice,
                    Benchmark = t.Benchmark,
                    TradeDate = t.TradeDate,
                    TradeSecurity = t.TradeSecurity,
                    TradeStatus = t.TradeStatus,
                    Trader = t.Trader,
                    Book = t.Book,
                    CreationName = t.CreationName,
                    CreationDate = t.CreationDate,
                    RevisionName = t.RevisionName,
                    RevisionDate = t.RevisionDate,
                    DealName = t.DealName,
                    DealType = t.DealType,
                    SourceListId = t.SourceListId,
                    Side = t.Side,
                    BuyCurrency = t.BuyCurrency,
                    SellCurrency = t.SellCurrency
                }).ToList();
                return Ok(tradeDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all trades");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/Trades/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TradeResponse>> GetTrade(int id)
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

                var tradeResponse = new TradeResponse
                {
                    TradeId = trade.TradeId,
                    Account = trade.Account,
                    AccountType = trade.AccountType,
                    BuyQuantity = trade.BuyQuantity,
                    SellQuantity = trade.SellQuantity,
                    BuyPrice = trade.BuyPrice,
                    SellPrice = trade.SellPrice,
                    Benchmark = trade.Benchmark,
                    TradeDate = trade.TradeDate,
                    TradeSecurity = trade.TradeSecurity,
                    TradeStatus = trade.TradeStatus,
                    Trader = trade.Trader,
                    Book = trade.Book,
                    CreationName = trade.CreationName,
                    CreationDate = trade.CreationDate,
                    RevisionName = trade.RevisionName,
                    RevisionDate = trade.RevisionDate,
                    DealName = trade.DealName,
                    DealType = trade.DealType,
                    SourceListId = trade.SourceListId,
                    Side = trade.Side,
                    BuyCurrency = trade.BuyCurrency,
                    SellCurrency = trade.SellCurrency
                };

                return Ok(tradeResponse);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching trade with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // POST: api/Trades
        [HttpPost]
        public async Task<IActionResult> CreateTrade(TradeRequest request)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid model state for TradeRequest");
                return BadRequest(ModelState);
            }

            try
            {
                _logger.LogInformation("Creating a new trade");
                var trade = request.ToTrade();
                await _tradeService.AddTradeAsync(trade);
                return Created ();
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
