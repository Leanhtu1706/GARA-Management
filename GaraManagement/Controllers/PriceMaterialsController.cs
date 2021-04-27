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
    public class PriceMaterialsController : Controller
    {
        private readonly GaraContext _context;

        public PriceMaterialsController(GaraContext context)
        {
            _context = context;
        }

        // GET: PriceMaterials
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("SessionUserName") == null || HttpContext.Session.GetString("PermissionAdmin") != "Yes")
            {
                if (HttpContext.Session.GetString("PermissionThuKho") != "Yes")
                {
                    return RedirectToAction("Index", "Login");
                }
            }
            var garaContext = _context.PriceMaterials.Include(p => p.IdMaterialNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: PriceMaterials/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceMaterial = await _context.PriceMaterials
                .Include(p => p.IdMaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceMaterial == null)
            {
                return NotFound();
            }

            return View(priceMaterial);
        }

        // GET: PriceMaterials/Create
        public IActionResult Create()
        {
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Id");
            return View();
        }

        // POST: PriceMaterials/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,IdMaterial,UpdateAt,Price")] PriceMaterial priceMaterial)
        {
            if (ModelState.IsValid)
            {
                _context.Add(priceMaterial);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Id", priceMaterial.IdMaterial);
            return View(priceMaterial);
        }

        // GET: PriceMaterials/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceMaterial = await _context.PriceMaterials.FindAsync(id);
            if (priceMaterial == null)
            {
                return NotFound();
            }
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Id", priceMaterial.IdMaterial);
            return View(priceMaterial);
        }

        // POST: PriceMaterials/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,IdMaterial,UpdateAt,Price")] PriceMaterial priceMaterial)
        {
            if (id != priceMaterial.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(priceMaterial);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PriceMaterialExists(priceMaterial.Id))
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
            ViewData["IdMaterial"] = new SelectList(_context.Materials, "Id", "Id", priceMaterial.IdMaterial);
            return View(priceMaterial);
        }

        // GET: PriceMaterials/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var priceMaterial = await _context.PriceMaterials
                .Include(p => p.IdMaterialNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (priceMaterial == null)
            {
                return NotFound();
            }

            return View(priceMaterial);
        }

        // POST: PriceMaterials/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var priceMaterial = await _context.PriceMaterials.FindAsync(id);
            _context.PriceMaterials.Remove(priceMaterial);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool PriceMaterialExists(int id)
        {
            return _context.PriceMaterials.Any(e => e.Id == id);
        }
    }
}
