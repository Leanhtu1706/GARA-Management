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
    public class DetailGoodsDeliveryNotesController : Controller
    {
        private readonly GaraContext _context;

        public DetailGoodsDeliveryNotesController(GaraContext context)
        {
            _context = context;
        }

        // GET: DetailGoodsDeliveryNotes
        public async Task<IActionResult> Index(int id)
        {
            var goodsDeliveryNotes = _context.GoodsDeliveryNotes.Include(a => a.IdRepairNavigation).Where(a => a.Id == id).FirstOrDefault();
            ViewBag.ExportDate = goodsDeliveryNotes.ExportDate;
            ViewBag.IdRepair = goodsDeliveryNotes.IdRepair;
            ViewBag.Description = goodsDeliveryNotes.Description;
            ViewBag.IdGoodDeliveryNotes = id;

            var garaContext = _context.DetailGoodsDeliveryNotes.Include(d => d.IdGoodsDeliveryNoteNavigation).Include(d => d.IdMaterialNavigation).Where(d=>d.IdGoodsDeliveryNote == id); 

            return View(await garaContext.ToListAsync());
        }

        // GET: DetailGoodsDeliveryNotes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailGoodsDeliveryNote = await _context.DetailGoodsDeliveryNotes
                .Include(d => d.IdGoodsDeliveryNoteNavigation)
                .Include(d => d.IdMaterialNavigation)
                .FirstOrDefaultAsync(m => m.IdGoodsDeliveryNote == id);
            if (detailGoodsDeliveryNote == null)
            {
                return NotFound();
            }

            return View(detailGoodsDeliveryNote);
        }

        // GET: DetailGoodsDeliveryNotes/Create
        public IActionResult Create(int id, string layout = "_")
        {
            //ViewData["IdGoodsDeliveryNote"] = new SelectList(_context.GoodsDeliveryNotes, "Id", "Id");
            var listMaterial = _context.Materials.Where(m => !_context.DetailGoodsDeliveryNotes.Any(dg => dg.IdGoodsDeliveryNote == id && dg.IdMaterial == m.Id));
            ViewData["MaterialName"] = new SelectList(listMaterial, "Id", "Name");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewBag.IdGoodDeliveryNotes = id;
       
            return View();
        }

        // POST: DetailGoodsDeliveryNotes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailGoodsDeliveryNote detailGoodsDeliveryNote)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailGoodsDeliveryNote);
                await _context.SaveChangesAsync();
                var idRepair = _context.GoodsDeliveryNotes.Include(a => a.IdRepairNavigation).Where(a => a.Id == detailGoodsDeliveryNote.IdGoodsDeliveryNote).FirstOrDefault().IdRepair;
                HttpContext.Session.SetString("SuccessMessage", "Thêm vật tư thành công");

                return RedirectToAction("Details","Repairs", new { id = idRepair });
            }
            ViewData["IdGoodsDeliveryNote"] = new SelectList(_context.GoodsDeliveryNotes, "Id", "Id", detailGoodsDeliveryNote.IdGoodsDeliveryNote);
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name");
            return View(detailGoodsDeliveryNote);
        }

        // GET: DetailGoodsDeliveryNotes/Edit/5
        public  IActionResult Edit(int? id, int idMaterial, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var detailGoodsDeliveryNote = _context.DetailGoodsDeliveryNotes.Where(a => a.IdGoodsDeliveryNote == id && a.IdMaterial == idMaterial).FirstOrDefault();
            if (detailGoodsDeliveryNote == null)
            {
                return NotFound();
            }
            ViewData["IdGoodsDeliveryNote"] = new SelectList(_context.GoodsDeliveryNotes, "Id", "Id", detailGoodsDeliveryNote.IdGoodsDeliveryNote);
            ViewData["MaterialName"] = new SelectList(_context.Materials, "Id", "Name", detailGoodsDeliveryNote.IdMaterial);
            return View(detailGoodsDeliveryNote);
        }

        // POST: DetailGoodsDeliveryNotes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idGoodsDeliveryNote, DetailGoodsDeliveryNote detailGoodsDeliveryNote)
        {
            if (idGoodsDeliveryNote != detailGoodsDeliveryNote.IdGoodsDeliveryNote)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailGoodsDeliveryNote);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "cập nhật vật tư thành công");   

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailGoodsDeliveryNoteExists(detailGoodsDeliveryNote.IdGoodsDeliveryNote))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                var idRepair = _context.GoodsDeliveryNotes.Include(a => a.IdRepairNavigation).Where(a => a.Id == detailGoodsDeliveryNote.IdGoodsDeliveryNote).FirstOrDefault().IdRepair;
                return RedirectToAction("Details", "Repairs", new { id = idRepair });
            }
            ViewData["IdGoodsDeliveryNote"] = new SelectList(_context.GoodsDeliveryNotes, "Id", "Id", detailGoodsDeliveryNote.IdGoodsDeliveryNote);
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Name", detailGoodsDeliveryNote.IdMaterial);
            return View(detailGoodsDeliveryNote);
        }

        //// GET: DetailGoodsDeliveryNotes/Delete/5
        //public async Task<IActionResult> Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return NotFound();
        //    }

        //    var detailGoodsDeliveryNote = await _context.DetailGoodsDeliveryNotes
        //        .Include(d => d.IdGoodsDeliveryNoteNavigation)
        //        .Include(d => d.IdMaterialNavigation)
        //        .FirstOrDefaultAsync(m => m.IdGoodsDeliveryNote == id);
        //    if (detailGoodsDeliveryNote == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(detailGoodsDeliveryNote);
        //}

        // POST: DetailGoodsDeliveryNotes/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int id, int idMaterial)
        {
            var detailGoodsDeliveryNote = _context.DetailGoodsDeliveryNotes.Where(a => a.IdGoodsDeliveryNote == id && a.IdMaterial == idMaterial).FirstOrDefault();
            _context.DetailGoodsDeliveryNotes.Remove(detailGoodsDeliveryNote);
            await _context.SaveChangesAsync();
            var idRepair = _context.GoodsDeliveryNotes.Include(a => a.IdRepairNavigation).Where(a => a.Id == detailGoodsDeliveryNote.IdGoodsDeliveryNote).FirstOrDefault().IdRepair;
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            return Json(new { redirectToUrl = Url.Action("Details", "Repairs", new { id = idRepair }) });
        }

        private bool DetailGoodsDeliveryNoteExists(int id)
        {
            return _context.DetailGoodsDeliveryNotes.Any(e => e.IdGoodsDeliveryNote == id);
        }
    }
}
