﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using X.PagedList;

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
        //public  IActionResult Index(int? pageNumber)
        //{
        //    if (pageNumber == null) pageNumber = 1;
        //    int pageSize = 10;
        //    var garaContext = _context.Supplies.Include(s => s.IdTypeNavigation);
            
        //    return View(garaContext.ToList().ToPagedList((int)pageNumber,pageSize));
        //}
        [HttpGet]
        public IActionResult Index(string search, int? pageNumber)
        {
            if (pageNumber == null) pageNumber = 1;
            int pageSize = 10;
            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Supplies.Include(s => s.IdTypeNavigation).Where(a => a.Name.Contains(search));
                return View(garaContext.ToList().ToPagedList((int)pageNumber, pageSize));
            }
            else
            {
                var garaContext = _context.Supplies.Include(s => s.IdTypeNavigation);
                return View(garaContext.ToList().ToPagedList((int)pageNumber, pageSize));
            }
            
        }
        // GET: Supplies/Details/5
        public async Task<IActionResult> Details(int? id)
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
        [HttpGet]
        public IActionResult Create(string layout = "_")
        {
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View();
        }

        // POST: Supplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Supply supply)
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
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            
            if (id == null)
            {
                return NotFound();
            }           
            var type =  _context.TypeOfSupplies.Select(i => i.Name).ToList();
            var image = _context.Supplies.Where(a => a.Id == id).Select(i => i.Image).FirstOrDefault();
            ViewBag.image = image;
            var supply = await _context.Supplies.FindAsync(id);
            if (supply == null)
            {
                return NotFound();
            }
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(supply);
        }

        // POST: Supplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Supply supply)
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
        public async Task<IActionResult> Delete(int? id)
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
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var supply = await _context.Supplies.FindAsync(id);
            _context.Supplies.Remove(supply);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SupplyExists(int? id)
        {
            return _context.Supplies.Any(e => e.Id == id);
        }
    }
}
