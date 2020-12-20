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
    public class WorksController : Controller
    {
        private readonly GaraContext _context;

        public WorksController(GaraContext context)
        {
            _context = context;
        }

        // GET: Works
        public async Task<IActionResult> Index(string search)
        {
            //var garaContext = _context.Works.Include(w => w.IdServiceNavigation);
            //return View(await garaContext.ToListAsync());
            if (HttpContext.Session.GetString("SessionUserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }

            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Works.Include(w => w.IdServiceNavigation).Where(a => a.IdServiceNavigation.Name.Contains(search) || a.WorkName.Contains(search));
                return View(await garaContext.ToListAsync());
            }
            else
            {
                var garaContext = _context.Works.Include(w => w.IdServiceNavigation);
                return View(await garaContext.ToListAsync());
            }
        }

        // GET: Works/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Works
                .Include(w => w.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // GET: Works/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");
            return View();
        }

        // POST: Works/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Work work)
        {
            if (ModelState.IsValid)
            {
                _context.Add(work);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");
            return View(work);
        }

        // GET: Works/Edit/5
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var work = await _context.Works.FindAsync(id);
            if (work == null)
            {
                return NotFound();
            }
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name", work.IdService);
            return View(work);
        }

        // POST: Works/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Work work)
        {
            if (id != work.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(work);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!WorkExists(work.Id))
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
            ViewData["IdService"] = new SelectList(_context.Services, "Id", "Name");
            return View(work);
        }

        // GET: Works/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var work = await _context.Works
                .Include(w => w.IdServiceNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (work == null)
            {
                return NotFound();
            }

            return View(work);
        }

        // POST: Works/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var work = await _context.Works.FindAsync(id);
            _context.Works.Remove(work);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool WorkExists(int id)
        {
            return _context.Works.Any(e => e.Id == id);
        }
    }
}
