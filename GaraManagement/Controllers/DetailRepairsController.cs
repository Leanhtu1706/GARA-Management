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
            ViewData["IdWork"] = new SelectList(_context.Works, "Id", "WorkName");
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
                return RedirectToAction("Details","Repairs", new {id = detailRepair.IdRepair });
            }
            ViewData["IdWork"] = new SelectList(_context.Works, "Id", "Id", detailRepair.IdWork);
            return View(detailRepair);
        }

        // GET: DetailRepairs/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var detailRepair = await _context.DetailRepairs.FindAsync(id);
            if (detailRepair == null)
            {
                return NotFound();
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", detailRepair.IdRepair);
            ViewData["IdWork"] = new SelectList(_context.Works, "Id", "Id", detailRepair.IdWork);
            return View(detailRepair);
        }

        // POST: DetailRepairs/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("IdRepair,IdWork")] DetailRepair detailRepair)
        {
            if (id != detailRepair.IdRepair)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(detailRepair);
                    await _context.SaveChangesAsync();
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
                return RedirectToAction(nameof(Index));
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
            var detailRepair = await _context.DetailRepairs.Where(d=>d.IdRepair == idRepair && d.IdWork == idWork).FirstOrDefaultAsync();
            _context.DetailRepairs.Remove(detailRepair);
            await _context.SaveChangesAsync();
            return Json(new { redirectToUrl = Url.Action("Details", "Repairs", new { id = detailRepair.IdRepair }) });
        }

        private bool DetailRepairExists(int id)
        {
            return _context.DetailRepairs.Any(e => e.IdRepair == id);
        }
    }
}
