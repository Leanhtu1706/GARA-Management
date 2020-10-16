using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;

namespace GaraManagement.Controllers
{
    public class SuppliesController : Controller
    {
        private readonly GaraContext _context;

        public SuppliesController(GaraContext context)
        {
            _context = context;
        }

        // GET: Supplies
        public  IActionResult Index()
        {
            var garaContext = _context.Supplies.Include(s => s.IdTypeNavigation);
            return View(garaContext);
        }

        // GET: Supplies/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supply = await _context.Supplies
                .Include(s => s.IdTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // GET: Supplies/Create
        public IActionResult Create()
        {
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdType,Name,Unit,Price,Amount,Description")] Supply supply)
        {
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            if (ModelState.IsValid)
            {
                _context.Add(supply);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View(supply);
        }

        // GET: Supplies/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var type =  _context.TypeOfSupplies.Select(i => i.Name).ToList();
            var supply = await _context.Supplies.FindAsync(id);
            if (supply == null)
            {
                return NotFound();
            }
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,IdType,Name,Unit,Price,Amount,Description")] Supply supply)
        {
            if (id != supply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(supply);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyExists(supply.Id))
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
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View(supply);
        }

        // GET: Supplies/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supply = await _context.Supplies
                .Include(s => s.IdTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // POST: Supplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var supply = await _context.Supplies.FindAsync(id);
            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyExists(string id)
        {
            return _context.Supplies.Any(e => e.Id == id);
        }
    }
}
