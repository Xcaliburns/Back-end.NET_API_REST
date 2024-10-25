using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Dot.Net.WebApi.Controllers;
using Dot.Net.WebApi.Data;

namespace Findexium.Api.Controllers
{
    public class RuleNameController : Controller
    {
        private readonly LocalDbContext _context;

        public RuleNameController(LocalDbContext context)
        {
            _context = context;
        }

        // GET: RuleName
        public async Task<IActionResult> Index()
        {
            return View(await _context.RuleNames.ToListAsync());
        }

        // GET: RuleName/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleName = await _context.RuleNames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruleName == null)
            {
                return NotFound();
            }

            return View(ruleName);
        }

        // GET: RuleName/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RuleName/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Json,Template,SqlStr,SqlPart")] RuleName ruleName)
        {
            if (ModelState.IsValid)
            {
                _context.Add(ruleName);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(ruleName);
        }

        // GET: RuleName/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleName = await _context.RuleNames.FindAsync(id);
            if (ruleName == null)
            {
                return NotFound();
            }
            return View(ruleName);
        }

        // POST: RuleName/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Json,Template,SqlStr,SqlPart")] RuleName ruleName)
        {
            if (id != ruleName.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(ruleName);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RuleNameExists(ruleName.Id))
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
            return View(ruleName);
        }

        // GET: RuleName/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var ruleName = await _context.RuleNames
                .FirstOrDefaultAsync(m => m.Id == id);
            if (ruleName == null)
            {
                return NotFound();
            }

            return View(ruleName);
        }

        // POST: RuleName/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var ruleName = await _context.RuleNames.FindAsync(id);
            if (ruleName != null)
            {
                _context.RuleNames.Remove(ruleName);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RuleNameExists(int id)
        {
            return _context.RuleNames.Any(e => e.Id == id);
        }
    }
}
