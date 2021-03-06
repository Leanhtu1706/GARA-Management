﻿using System;
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
    public class DetailRepairsController : Controller
    {
        private readonly GaraContext _context;

        public DetailRepairsController(GaraContext context)
        {
            _context = context;
        }

        // GET: DetailRepairs
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            if (HttpContext.Session.GetString("ErrorMessage") != null)
            {
                ViewBag.ErrorMessage = HttpContext.Session.GetString("ErrorMessage");
                HttpContext.Session.Remove("ErrorMessage");
            }
            var garaContext = _context.DetailRepairs.Include(d => d.IdRepairNavigation).Include(d => d.IdWorkNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: DetailRepairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailRepair = await _context.DetailRepairs
                .Include(d => d.IdRepairNavigation)
                .Include(d => d.IdWorkNavigation)
                .FirstOrDefaultAsync(m => m.IdRepair == id);
            if (detailRepair == null)
            {
                return NotFound();
            }

            return View(detailRepair);
        }

        // GET: DetailRepairs/Create
        public IActionResult Create(int idRepair, string layout = "_")
        {

            var workList = _context.Works.Where(w => !_context.DetailRepairs.Any(dr => dr.IdRepair == idRepair && dr.IdWork == w.Id)).ToList();

            ViewData["IdWork"] = new SelectList(workList, "Id", "WorkName");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            DetailRepair detailRepair = new DetailRepair();
            detailRepair.IdRepair = idRepair;
            return View(detailRepair);
        }

        // POST: DetailRepairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(DetailRepair detailRepair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(detailRepair);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm công việc thành công");

                return RedirectToAction("Details", "Repairs", new { id = detailRepair.IdRepair });
            }
            var workList = _context.Works.Where(w => !_context.DetailRepairs.Any(dr => dr.IdRepair == detailRepair.IdRepair && dr.IdWork == w.Id)).ToList();

            ViewData["IdWork"] = new SelectList(workList, "Id", "WorkName");
            return View(detailRepair);
        }

        // GET: DetailRepairs/Edit/5
        public async Task<IActionResult> Edit(int? id, int idWork, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var detailRepair = await _context.DetailRepairs.Where(dr => dr.IdRepair == id && dr.IdWork == idWork).FirstAsync();
            if (detailRepair == null)
            {
                return NotFound();
            }

            ViewData["IdWork"] = new SelectList(_context.Works, "Id", "WorkName");

            return View(detailRepair);
        }

        // POST: DetailRepairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int idRepair, DetailRepair detailRepair)
        {
            if (idRepair != detailRepair.IdRepair)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailRepair);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DetailRepairExists(detailRepair.IdRepair))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Details", "Repairs", new { id = idRepair });
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", detailRepair.IdRepair);
            ViewData["IdWork"] = new SelectList(_context.Works, "Id", "Id", detailRepair.IdWork);
            return View(detailRepair);
        }

        // GET: DetailRepairs/Delete/5
        //public async Task<IActionResult> Delete(int? idRepair, int idWork)
        //{
        //    if (idRepair == null)
        //    {
        //        return NotFound();
        //    }

        //    var detailRepair = await _context.DetailRepairs
        //        .Include(d => d.IdRepairNavigation)
        //        .Include(d => d.IdWorkNavigation)
        //        .Where(m => m.IdRepair == idRepair && m.IdWork == idWork).FirstOrDefaultAsync();
        //    if (detailRepair == null)
        //    {
        //        return NotFound();
        //    }

        //    return View(detailRepair);
        //}

        // POST: DetailRepairs/Delete/5
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? idRepair, int idWork)
        {

            try
            {
                var detailRepair = await _context.DetailRepairs.Where(d => d.IdRepair == idRepair && d.IdWork == idWork).FirstOrDefaultAsync();             
                _context.DetailRepairs.Remove(detailRepair);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            }
            catch
            {
                HttpContext.Session.SetString("ErrorMessage", "Không thể xóa do vật tư còn ràng buộc với các bảng khác!");
            }


            return Json(new { redirectToUrl = Url.Action("Details", "Repairs", new { id = idRepair }) });
        }

        private bool DetailRepairExists(int id)
        {
            return _context.DetailRepairs.Any(e => e.IdRepair == id);
        }
    }
}
