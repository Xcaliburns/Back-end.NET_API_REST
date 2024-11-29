using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Findexium.Infrastructure;
using Findexium.Domain.Interfaces;
using Findexium.Domain.Models;
using Findexium.Api.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Cryptography;
using System.Configuration;
using System.Data;
using System.Net.NetworkInformation;

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [Authorize(Roles = "User,Admin")]
    [ApiController]
   
    public class BidListController : ControllerBase
    {
        private readonly IBidListServices _bidListServices;
        private readonly ILogger<BidListController> _logger;

        public BidListController(IBidListServices bidListServices, ILogger<BidListController> logger)
        {
            _bidListServices = bidListServices;
            _logger = logger;
        }

        // GET: api/BidList
        [HttpGet]

        public async Task<ActionResult<IEnumerable<BidResponse>>> GetBids()
        {
            try
            {
                _logger.LogInformation("Fetching all bids");
                var bids = await _bidListServices.GetAllAsync();
                var bidDtos = bids.Select(b => new BidResponse
                {
                    BidListId = b.BidListId,
                    Account = b.Account,
                    BidType = b.BidType,
                    BidQuantity = b.BidQuantity,
                }).ToList();
                return Ok(bidDtos);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching all bids");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // GET: api/BidLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetBid(int id)
        {
            try
            {
                _logger.LogInformation("Fetching bid with id: {Id}", id);
                var bid = await _bidListServices.GetByIdAsync(id);
                if (bid == null)
                {
                    _logger.LogWarning("Bid with id: {Id} not found", id);
                    return NotFound();
                }
               
                return Ok(bid);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching bid with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }

        // PUT: api/BidLists/5
        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> PutBid(int id, BidList bidList)
        {
            if (id != bidList.BidListId)
            {
                return BadRequest();
            }

            try
            {
                _logger.LogInformation("Updating bid with id: {Id}", id);
                await _bidListServices.UpdateAsync(bidList);
            }
            //TODO :a placer dans l'infrastructure
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bidListServices.ExistsAsync(id))
                {
                    _logger.LogWarning("Bid with id: {Id} not found during update", id);
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating bid with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }

            return NoContent();
        }

        // POST: api/BidLists
        [HttpPost]
        [Authorize]
        public async Task<IActionResult> PostBidList(BidRequest request)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _logger.LogWarning("Invalid model state for BidRequest");
                    return BadRequest(ModelState);
                }
                _logger.LogInformation("Creating a new bid");
                var bid = request.ToBid();
                await _bidListServices.AddAsync(bid);
                
                return Created();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while creating a new bid");
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }


        // DELETE: api/BidLists/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            try
            {
                _logger.LogInformation("Deleting bid with id: {Id}", id);
                var bidList = await _bidListServices.GetByIdAsync(id);
                if (bidList == null)
                {
                    _logger.LogWarning("Bid with id: {Id} not found during deletion", id);
                    return NotFound();
                }

                await _bidListServices.DeleteAsync(id);
                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while deleting bid with id: {Id}", id);
                return StatusCode(StatusCodes.Status500InternalServerError, "Internal server error");
            }
        }
    }
}
