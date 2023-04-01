using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ASP.NET_TestApp.Models;
using ASP.NET_TestApp.Interfaces;

namespace ASP.NET_TestApp.Controllers
{
    public class BetsController : Controller
    {
        private readonly IDataService _dataService;
        private readonly PariContext _context;

        public BetsController(PariContext context, IDataService dataService)
        {
            _dataService = dataService;
            _context = context;
        }

        // GET: Bets
        public async Task<IActionResult> Index()
        {
              return _context.Bets != null ? 
                          View(await _context.Bets.ToListAsync().ConfigureAwait(false)) :
                          Problem("Entity set 'PariContext.Bets'  is null.");
        }

        // GET: Bets/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Bets == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // GET: Bets/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Bets/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,PlayerId,Amount,Gain,Date,SettlementDate")] Bet bet)
        {
            if (ModelState.IsValid)
            {
                _context.Add(bet);
                await _dataService.RecalculateBalanceAsync(bet).ConfigureAwait(false);
                await _context.SaveChangesAsync().ConfigureAwait(false);
                
                
                return RedirectToAction(nameof(Index));
            }
            return View(bet);
        }

        // GET: Bets/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Bets == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets.FindAsync(id).ConfigureAwait(false);
            if (bet == null)
            {
                return NotFound();
            }
            return View(bet);
        }

        // POST: Bets/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,PlayerId,Amount,Gain,Date,SettlementDate")] Bet bet)
        {
            if (id != bet.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    await _dataService.RecalculateBalanceAsync(bet).ConfigureAwait(false);
                    _context.Update(bet);
                    await _context.SaveChangesAsync().ConfigureAwait(false);
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BetExists(bet.Id))
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
            return View(bet);
        }

        // GET: Bets/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Bets == null)
            {
                return NotFound();
            }

            var bet = await _context.Bets
                .FirstOrDefaultAsync(m => m.Id == id).ConfigureAwait(false);
            if (bet == null)
            {
                return NotFound();
            }

            return View(bet);
        }

        // POST: Bets/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Bets == null)
            {
                return Problem("Entity set 'PariContext.Bets'  is null.");
            }
            var bet = await _context.Bets.FindAsync(id);
            if (bet != null)
            {
                await _dataService.RecalculateBalanceAsync(bet, betDeleted: true).ConfigureAwait(false);
                _context.Bets.Remove(bet);
                await _context.SaveChangesAsync().ConfigureAwait(false);
            }
            
            
            
            return RedirectToAction(nameof(Index));
        }

        private bool BetExists(int id)
        {
          return (_context.Bets?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
