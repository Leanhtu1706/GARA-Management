using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace GaraManagement.Controllers
{
    public class GoodsDeliveryNotesController : Controller
    {
        private readonly GaraContext _context;

        public GoodsDeliveryNotesController(GaraContext context)
        {
            _context = context;
        }

        // GET: GoodsDeliveryNotes
        public IActionResult Index(string search)
        {
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.GoodsDeliveryNotes.Include(g => g.IdRepairNavigation).Include(e => e.IdEmployeeNavigation).Where(a => a.IdRepairNavigation.IdCarNavigation.CarName.Contains(search));
                return View(garaContext.ToList());
            }
            else
            {
                var garaContext = _context.GoodsDeliveryNotes.Include(g => g.IdRepairNavigation).Include(e => e.IdEmployeeNavigation);
                return View(garaContext.ToList());
            }
        }
        // GET: GoodsDeliveryNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsDeliveryNote = await _context.GoodsDeliveryNotes
                .Include(g => g.IdRepairNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsDeliveryNote == null)
            {
                return NotFound();
            }

            return View(goodsDeliveryNote);
        }

        // GET: GoodsDeliveryNotes/Create
        public IActionResult Create(int? idRepair , string layout = "_", string title = "")
        {
      
            ViewData["IdRepair"] = idRepair;
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["Title"] = title;
            return View();
        }

        // POST: GoodsDeliveryNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(GoodsDeliveryNote goodsDeliveryNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(goodsDeliveryNote);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Yêu cầu vật tư thành công");

                return RedirectToAction("Details", "Repairs", new { id = goodsDeliveryNote.IdRepair });
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", goodsDeliveryNote.IdRepair);
            return View(goodsDeliveryNote);
        }

        // GET: GoodsDeliveryNotes/Edit/5
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var goodsDeliveryNote = await _context.GoodsDeliveryNotes.FindAsync(id);
            if (goodsDeliveryNote == null)
            {
                return NotFound();
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", goodsDeliveryNote.IdRepair);
            return View(goodsDeliveryNote);
        }

        // POST: GoodsDeliveryNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ExportDate,IdRepair,Description")] GoodsDeliveryNote goodsDeliveryNote)
        {
            if (id != goodsDeliveryNote.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    goodsDeliveryNote.UpdateAt = DateTime.Now;
                    _context.Update(goodsDeliveryNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoodsDeliveryNoteExists(goodsDeliveryNote.Id))
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
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", goodsDeliveryNote.IdRepair);
            return View(goodsDeliveryNote);
        }

        // GET: GoodsDeliveryNotes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goodsDeliveryNote = await _context.GoodsDeliveryNotes
                .Include(g => g.IdRepairNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (goodsDeliveryNote == null)
            {
                return NotFound();
            }

            return View(goodsDeliveryNote);
        }

        // POST: GoodsDeliveryNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goodsDeliveryNote = await _context.GoodsDeliveryNotes.FindAsync(id);
            var idRepair = goodsDeliveryNote.IdRepair;
            _context.GoodsDeliveryNotes.Remove(goodsDeliveryNote);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Hủy yêu cầu xuất vật tư thành công");
            return Json(new { redirectToUrl = Url.Action("Details", "Repairs", new { id = idRepair }) });
        }

        private bool GoodsDeliveryNoteExists(int id)
        {
            return _context.GoodsDeliveryNotes.Any(e => e.Id == id);
        }
    }
}
