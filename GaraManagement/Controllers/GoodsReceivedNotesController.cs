using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using X.PagedList;

namespace GaraManagement.Controllers
{
    public class GoodsReceivedNotesController : Controller
    {
        private readonly GaraContext _context;

        public GoodsReceivedNotesController(GaraContext context)
        {
            _context = context;
        }

        // GET: GoodsReceivedNotes
        public IActionResult Index(string search)
        {

            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.GoodsReceivedNotes.Include(g => g.IdSupplierNavigation).Where(a => a.IdSupplierNavigation.Name.Contains(search));
                return View(garaContext.ToList());
            }
            else
            {
                var garaContext = _context.GoodsReceivedNotes.Include(g => g.IdSupplierNavigation);
                return View(garaContext.ToList());
            }
        }
        // GET: GoodsReceivedNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceivedNote = await _context.GoodsReceivedNotes
                .Include(g => g.IdSupplierNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsReceivedNote == null)
            {
                return NotFound();
            }

            return View(goodsReceivedNote);
        }

        // GET: GoodsReceivedNotes/Create
        public IActionResult Create(string layout = "_")
        {
            var supplier = _context.Suppliers.Select(i => i.Name).ToList();
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "Name",supplier);
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: GoodsReceivedNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GoodsReceivedNote goodsReceivedNote)
        {
            if (ModelState.IsValid)
            {                
                _context.Add(goodsReceivedNote);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            var supplier = _context.Suppliers.Select(i => i.Name).ToList();
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "Name", supplier);
            return View(goodsReceivedNote);
        }

        // GET: GoodsReceivedNotes/Edit/5
        public async Task<IActionResult> Edit(int? id,string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var goodsReceivedNote = await _context.GoodsReceivedNotes.FindAsync(id);
            if (goodsReceivedNote == null)
            {
                return NotFound();
            }
            var supplier = _context.Suppliers.Select(i => i.Name).ToList();
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "Name", supplier);
            return View(goodsReceivedNote);
        }

        // POST: GoodsReceivedNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, GoodsReceivedNote goodsReceivedNote)
        {
            if (id != goodsReceivedNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    goodsReceivedNote.UpdateAt = DateTime.Now;
                    _context.Update(goodsReceivedNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsReceivedNoteExists(goodsReceivedNote.Id))
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
            var supplier = _context.Suppliers.Select(i => i.Name).ToList();
            ViewData["IdSupplier"] = new SelectList(_context.Suppliers, "Id", "Name", supplier);
            return View(goodsReceivedNote);
        }

        // GET: GoodsReceivedNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsReceivedNote = await _context.GoodsReceivedNotes
                .Include(g => g.IdSupplierNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsReceivedNote == null)
            {
                return NotFound();
            }

            return View(goodsReceivedNote);
        }

        // POST: GoodsReceivedNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsReceivedNote = await _context.GoodsReceivedNotes.FindAsync(id);
            _context.GoodsReceivedNotes.Remove(goodsReceivedNote);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoodsReceivedNoteExists(int id)
        {
            return _context.GoodsReceivedNotes.Any(e => e.Id == id);
        }
    }
}
