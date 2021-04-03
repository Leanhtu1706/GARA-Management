using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;

namespace GaraManagement.Controllers
{
    public class HistoriesController : Controller
    {
        private readonly GaraContext _context;

        public HistoriesController(GaraContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            var garaContext = _context.Historys.Include(h => h.UserNameNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: Histories/Details/5
        //    public async Task<IActionResult> Details(DateTime? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var history = await _context.Historys
        //            .Include(h => h.IdEmployeeNavigation)
        //            .FirstOrDefaultAsync(m => m.DateHistory == id);
        //        if (history == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(history);
        //    }

        //    // GET: Histories/Create
        //    public IActionResult Create()
        //    {
        //        ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id");
        //        return View();
        //    }

        //    // POST: Histories/Create
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Create([Bind("DateHistory,Event,IdEmployee")] History history)
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            _context.Add(history);
        //            await _context.SaveChangesAsync();
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id", history.IdEmployee);
        //        return View(history);
        //    }

        //    // GET: Histories/Edit/5
        //    public async Task<IActionResult> Edit(DateTime? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var history = await _context.Historys.FindAsync(id);
        //        if (history == null)
        //        {
        //            return NotFound();
        //        }
        //        ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id", history.IdEmployee);
        //        return View(history);
        //    }

        //    // POST: Histories/Edit/5
        //    // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        //    // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        //    [HttpPost]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> Edit(DateTime id, [Bind("DateHistory,Event,IdEmployee")] History history)
        //    {
        //        if (id != history.DateHistory)
        //        {
        //            return NotFound();
        //        }

        //        if (ModelState.IsValid)
        //        {
        //            try
        //            {
        //                _context.Update(history);
        //                await _context.SaveChangesAsync();
        //            }
        //            catch (DbUpdateConcurrencyException)
        //            {
        //                if (!HistoryExists(history.DateHistory))
        //                {
        //                    return NotFound();
        //                }
        //                else
        //                {
        //                    throw;
        //                }
        //            }
        //            return RedirectToAction(nameof(Index));
        //        }
        //        ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id", history.IdEmployee);
        //        return View(history);
        //    }

        //    // GET: Histories/Delete/5
        //    public async Task<IActionResult> Delete(DateTime? id)
        //    {
        //        if (id == null)
        //        {
        //            return NotFound();
        //        }

        //        var history = await _context.Historys
        //            .Include(h => h.IdEmployeeNavigation)
        //            .FirstOrDefaultAsync(m => m.DateHistory == id);
        //        if (history == null)
        //        {
        //            return NotFound();
        //        }

        //        return View(history);
        //    }

        //    // POST: Histories/Delete/5
        //    [HttpPost, ActionName("Delete")]
        //    [ValidateAntiForgeryToken]
        //    public async Task<IActionResult> DeleteConfirmed(DateTime id)
        //    {
        //        var history = await _context.Historys.FindAsync(id);
        //        _context.Historys.Remove(history);
        //        await _context.SaveChangesAsync();
        //        return RedirectToAction(nameof(Index));
        //    }

        //    private bool HistoryExists(DateTime id)
        //    {
        //        return _context.Historys.Any(e => e.DateHistory == id);
        //    }
    }
}
