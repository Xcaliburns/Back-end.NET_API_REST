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

namespace Findexium.Api.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly IBidListServices _bidListServices;

        public BidListController(IBidListServices bidListServices)
        {
            _bidListServices = bidListServices;
        }

        // GET: api/BidList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidList>>> GetBids()
        {
            return Ok(await _bidListServices.GetAllAsync());
        }

        // GET: api/BidLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetBid(int id)
        {
            var bid = await _bidListServices.GetByIdAsync(id);
            if (bid == null)
            {
                return NotFound();
            }
            return bid;
        }

        // PUT: api/BidLists/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutBid(int id, BidList bidList)
        {
            if (id != bidList.BidListId)
            {
                return BadRequest();
            }

            try
            {
                await _bidListServices.UpdateAsync(bidList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bidListServices.ExistsAsync(id))
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

        // POST: api/BidLists
        [HttpPost]
        public async Task<ActionResult<BidList>> PostBidList(BidList bidList)
        {
            await _bidListServices.AddAsync(bidList);
            return CreatedAtAction("GetBid", new { id = bidList.BidListId }, bidList);
        }

        // DELETE: api/BidLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            var bidList = await _bidListServices.GetByIdAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }

            await _bidListServices.DeleteAsync(id);
            return NoContent();
        }
    }
}
