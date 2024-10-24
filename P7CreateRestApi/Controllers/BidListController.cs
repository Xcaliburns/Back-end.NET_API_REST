using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Data;
using Dot.Net.WebApi.Domain;
using P7CreateRestApi.Interfaces;
using P7CreateRestApi.Repositories;

namespace P7CreateRestApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BidListController : ControllerBase
    {
        private readonly IBidListRepository _repository;

        public BidListController(IBidListRepository repository)
        {
            _repository = repository;
        }

        // GET: api/BidList
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BidList>>> GetBids()
        {
            return Ok(await _repository.GetAllAsync());
        }

        // GET: api/BidLists/5
        [HttpGet("{id}")]
        public async Task<ActionResult<BidList>> GetBid(int id)
        {
            var bid = await _repository.GetByIdAsync(id);
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
                await _repository.UpdateAsync(bidList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _repository.ExistsAsync(id))
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
            await _repository.AddAsync(bidList);
            return CreatedAtAction("GetBid", new { id = bidList.BidListId }, bidList);
        }

        // DELETE: api/BidLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            var bidList = await _repository.GetByIdAsync(id);
            if (bidList == null)
            {
                return NotFound();
            }

            await _repository.DeleteAsync(id);
            return NoContent();
        }
    }
}
