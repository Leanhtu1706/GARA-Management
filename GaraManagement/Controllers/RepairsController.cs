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
    public class RepairsController : Controller
    {
        private readonly GaraContext _context;

        public RepairsController(GaraContext context)
        {
            _context = context;
        }

        // GET: Repairs
        public async Task<IActionResult> Index(int? IdCar, string date, StateType? state)
        {
            if (HttpContext.Session.GetString("SessionUserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            //ViewData["Car"] = _context.Cars.Where(c => c.Id == IdCar).Select(c=>c.CarName);
            ViewData["IdCar"] = new SelectList(_context.Cars.Where(c => c.Id == IdCar), "Id", "CarName");
            //ViewData["LicensePlates"] = new SelectList(_context.Cars, "Id", "LicensePlates");
            if (date != null)
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Where(r => r.DateOfFactoryEntry.ToString().Contains(date)).OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else if (state != null)
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Where(r => r.State == state).OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }

        }

        // GET: Repairs/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .Include(r => r.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair == null)
            {
                return NotFound();
            }
            else
            {
                if (repair.GoodsDeliveryNotes.Count == 0)
                {
                    ViewData["checkGoodsDeliveryNotes"] = "Null";
                }
                else
                {
                    ViewData["checkGoodsDeliveryNotes"] = "notNull";
                }

                return View(repair);
            }
        }

        // GET: Repairs/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Repairs/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdCar,DateOfFactoryEntry,DateFinished,State")] Repair repair)
        {
            if (ModelState.IsValid)
            {
                _context.Add(repair);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            return View(repair);
        }

        // GET: Repairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs.FindAsync(id);
            if (repair == null)
            {
                return NotFound();
            }
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            return View(repair);
        }

        // POST: Repairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdCar,DateOfFactoryEntry,DateFinished,State")] Repair repair)
        {
            if (id != repair.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(repair);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RepairExists(repair.Id))
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
            ViewData["IdCar"] = new SelectList(_context.Cars, "Id", "Id", repair.IdCar);
            return View(repair);
        }

        // GET: Repairs/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair == null)
            {
                return NotFound();
            }

            return Json(repair.Id);
        }

        // POST: Repairs/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var repair = await _context.Repairs.FindAsync(id);
            var goodsDeliveryNote = _context.GoodsDeliveryNotes.Where(g => g.IdRepair == id).FirstOrDefault();
            var detailGoodsDeliveryNote = _context.DetailGoodsDeliveryNotes.Include(dg => dg.IdGoodsDeliveryNoteNavigation).Where(dg => dg.IdGoodsDeliveryNote == goodsDeliveryNote.Id).ToList();
            var detailRepair = _context.DetailRepairs.Where(dr => dr.IdRepair == id).ToList();
            foreach(var itemdr in detailRepair)
            {
                _context.DetailRepairs.Remove(itemdr);
            }    
            foreach(var itemdg in detailGoodsDeliveryNote)
            {
                _context.DetailGoodsDeliveryNotes.Remove(itemdg);
            }    
            _context.GoodsDeliveryNotes.Remove(goodsDeliveryNote);
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RepairExists(int id)
        {
            return _context.Repairs.Any(e => e.Id == id);
        }
        public async Task<IActionResult> BaoGia(int? id)
        {
            var repair = await _context.Repairs
                .Include(r => r.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .Include(r => r.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            return View(repair);
        }
    }
}
