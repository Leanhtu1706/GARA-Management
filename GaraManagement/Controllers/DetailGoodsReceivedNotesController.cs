using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Http;

namespace GaraManagement.Controllers
{
    public class DetailGoodsReceivedNotesController : Controller
    {
        private readonly GaraContext _context;

        public DetailGoodsReceivedNotesController(GaraContext context)
        {
            _context = context;
        }

        // GET: DetailGoodsReceivedNotes
        public async Task<IActionResult> Index(int id)
        {
            var goodsReceivedNotes = _context.GoodsReceivedNotes.Include(a=>a.IdSupplierNavigation).Where(a => a.Id == id).FirstOrDefault();
            ViewBag.ImportDate = goodsReceivedNotes.ImportDate;
            ViewBag.SupplierName = goodsReceivedNotes.IdSupplierNavigation.Name;
            ViewBag.Description = goodsReceivedNotes.Description;
            ViewBag.IdGoodReceivedNotes = id;
            var garaContext = _context.DetailGoodsReceivedNotes.Include(d => d.IdGoodsReceivedNoteNavigation).Include(d => d.IdMaterialNavigation).Where(a=>a.IdGoodsReceivedNote == id);
            return View(await garaContext.ToListAsync());
        }

        // GET: DetailGoodsReceivedNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailGoodsReceivedNote = await _context.DetailGoodsReceivedNotes
                .Include(d => d.IdGoodsReceivedNoteNavigation)
                .Include(d => d.IdMaterialNavigation)
                .FirstOrDefaultAsync(m => m.IdGoodsReceivedNote == id);
            if (detailGoodsReceivedNote == null)
            {
                return NotFound();
            }

            return View(detailGoodsReceivedNote);
        }

        // GET: DetailGoodsReceivedNotes/Create
        public IActionResult Create(int id,string layout = "_")
        {
            ViewData["IdGoodsReceivedNote"] = new SelectList(_context.GoodsReceivedNotes, "Id", "Id");           
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewBag.IdGoodReceivedNotes = id;
            return View();
        }

        // POST: DetailGoodsReceivedNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailGoodsReceivedNote detailGoodsReceivedNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailGoodsReceivedNote);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                return RedirectToAction(nameof(Index), new {id = detailGoodsReceivedNote.IdGoodsReceivedNote });
            }
            ViewData["IdGoodsReceivedNote"] = new SelectList(_context.GoodsReceivedNotes, "Id", "Id", detailGoodsReceivedNote.IdGoodsReceivedNote);
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name");
            return View(detailGoodsReceivedNote);
        }

        // GET: DetailGoodsReceivedNotes/Edit/5
        public IActionResult Edit(int? id, int idMaterial, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var detailGoodsReceivedNote =  _context.DetailGoodsReceivedNotes.Where(a=>a.IdGoodsReceivedNote == id && a.IdMaterial == idMaterial).FirstOrDefault();
            if (detailGoodsReceivedNote == null)
            {
                return NotFound();
            }
            ViewData["IdGoodsReceivedNote"] = new SelectList(_context.GoodsReceivedNotes, "Id", "Id", detailGoodsReceivedNote.IdGoodsReceivedNote);
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name");
            return View(detailGoodsReceivedNote);
        }

        // POST: DetailGoodsReceivedNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idGoodsReceivedNote, DetailGoodsReceivedNote detailGoodsReceivedNote)
        {
            if (idGoodsReceivedNote != detailGoodsReceivedNote.IdGoodsReceivedNote)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailGoodsReceivedNote);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailGoodsReceivedNoteExists(detailGoodsReceivedNote.IdGoodsReceivedNote))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { id = detailGoodsReceivedNote.IdGoodsReceivedNote });
            }
            ViewData["IdGoodsReceivedNote"] = new SelectList(_context.GoodsReceivedNotes, "Id", "Id", detailGoodsReceivedNote.IdGoodsReceivedNote);
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name");
            return View(detailGoodsReceivedNote);
        }

        // GET: DetailGoodsReceivedNotes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var detailGoodsReceivedNote = await _context.DetailGoodsReceivedNotes
        //        .Include(d => d.IdGoodsReceivedNoteNavigation)
        //        .Include(d => d.IdMaterialNavigation)
        //        .FirstOrDefaultAsync(m => m.IdGoodsReceivedNote == id);
        //    if (detailGoodsReceivedNote == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(detailGoodsReceivedNote);
        //}

        // POST: DetailGoodsReceivedNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int idMaterial)
        {
            var detailGoodsReceivedNote =  _context.DetailGoodsReceivedNotes.Where(a=>a.IdGoodsReceivedNote == id && a.IdMaterial == idMaterial).FirstOrDefault();
            _context.DetailGoodsReceivedNotes.Remove(detailGoodsReceivedNote);
            await _context.SaveChangesAsync();
            return Json(new { redirectToUrl = Url.Action("Index", "DetailGoodsReceivedNotes", new {id = id }) });
        }

        private bool DetailGoodsReceivedNoteExists(int id)
        {
            return _context.DetailGoodsReceivedNotes.Any(e => e.IdGoodsReceivedNote == id);
        }
    }
}
