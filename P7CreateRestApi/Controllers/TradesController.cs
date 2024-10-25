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

namespace Findexium.Api.Controllers
{
    public class TradesController : Controller
    {
        private readonly ITradeService _tradeService;

        public TradesController(ITradeService tradeService)
        {
            _tradeService = tradeService;
        }

        // GET: Trades
        public async Task<IActionResult> Index()
        {
            return View(await _tradeService.GetAllTradesAsync());
        }

        // GET: Trades/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trade = await _tradeService.GetTradeByIdAsync(id.Value);
            if (trade == null)
            {
                return NotFound();
            }

            return View(trade);
        }

        // GET: Trades/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Trades/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TradeId,Account,AccountType,BuyQuantity,SellQuantity,BuyPrice,SellPrice,TradeDate,TradeSecurity,TradeStatus,Trader,Benchmark,Book,CreationName,CreationDate,RevisionName,RevisionDate,DealName,DealType,SourceListId,Side")] Trade trade)
        {
            if (ModelState.IsValid)
            {
                await _tradeService.AddTradeAsync(trade);
                return RedirectToAction(nameof(Index));
            }
            return View(trade);
        }

        // GET: Trades/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trade = await _tradeService.GetTradeByIdAsync(id.Value);
            if (trade == null)
            {
                return NotFound();
            }
            return View(trade);
        }

        // POST: Trades/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TradeId,Account,AccountType,BuyQuantity,SellQuantity,BuyPrice,SellPrice,TradeDate,TradeSecurity,TradeStatus,Trader,Benchmark,Book,CreationName,CreationDate,RevisionName,RevisionDate,DealName,DealType,SourceListId,Side")] Trade trade)
        {
            if (id != trade.TradeId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _tradeService.UpdateTradeAsync(trade);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _tradeService.TradeExistsAsync(trade.TradeId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(trade);
        }

        // GET: Trades/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var trade = await _tradeService.GetTradeByIdAsync(id.Value);
            if (trade == null)
            {
                return NotFound();
            }

            return View(trade);
        }

        // POST: Trades/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _tradeService.DeleteTradeAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
