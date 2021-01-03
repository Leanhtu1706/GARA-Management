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
    public class PaysController : Controller
    {
        private readonly GaraContext _context;

        public PaysController(GaraContext context)
        {
            _context = context;
        }

        // GET: Pays
        public async Task<IActionResult> Index()
        {
            var garaContext = _context.Pays.Include(p => p.IdRepairNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: Pays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.IdRepairNavigation.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .Include(r => r.IdRepairNavigation.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // GET: Pays/Create
        public IActionResult Create(int idRepair, string layout = "_")
        {
            ViewData["IdRepair"] = idRepair;
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: Pays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Pay pay)
        {
            if (ModelState.IsValid)
            {
                _context.Add(pay);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Edit/5
        public async Task<IActionResult> Edit(int? idRepair, string layout = "_")
        {
            if (idRepair == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays.Where(p => p.IdRepair == idRepair).FirstOrDefaultAsync();
            if (pay == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pay pay)
        {
            if (id != pay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pay.Paid += pay.owe;
                    pay.Update_at = DateTime.Now;
                    _context.Update(pay);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayExists(pay.Id))
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
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // POST: Pays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pay = await _context.Pays.FindAsync(id);
            _context.Pays.Remove(pay);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PayExists(int id)
        {
            return _context.Pays.Any(e => e.Id == id);
        }
    }
}
