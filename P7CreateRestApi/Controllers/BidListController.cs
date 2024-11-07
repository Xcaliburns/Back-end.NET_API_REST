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
            var bids = await _bidListServices.GetAllAsync();
            return Ok(bids);
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
            return Ok(bid);
        }

        // PUT: api/BidLists/5
        [HttpPut("{id}")]
        [Authorize] //TODO ajouter les attributs    pour l'authentification
        public async Task<IActionResult> PutBid(int id, BidList bidList)
        {
            if (id != bidList.BidListId)
            {
                return BadRequest();//400
            }

            try
            {
                await _bidListServices.UpdateAsync(bidList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!await _bidListServices.ExistsAsync(id))
                {
                    return NotFound();//404
                }
                else
                {
                    throw;
                }
            }

            return NoContent();//204
        }

        // POST: api/BidLists
        //TODO : modifier les autres methodes post des controller pour ressembler  à celle ci (modif sur le getBid également)
        [HttpPost]
        [Authorize]
        public async Task<ActionResult<BidList>> PostBidList(BidRequest request)
        {
            var bidList = request.ToBid();
            await _bidListServices.AddAsync(bidList);

            // Assuming you have a GetBid action that takes an id parameter
            return CreatedAtAction(nameof(GetBid), new { id = bidList.BidListId }, bidList);
        }

        // DELETE: api/BidLists/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBidList(int id)
        {
            var bidList = await _bidListServices.GetByIdAsync(id);
            if (bidList == null)
            {
                return NotFound(); //404
            }

            await _bidListServices.DeleteAsync(id);
            return NoContent();//204
        }
    }
}
