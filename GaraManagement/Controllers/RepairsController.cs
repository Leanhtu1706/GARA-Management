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
    public class RepairsController : Controller
    {
        private readonly GaraContext _context;

        public RepairsController(GaraContext context)
        {
            _context = context;
        }

        // GET: Repairs
        public async Task<IActionResult> Index(int? IdCar, string date, StateType? state, string search)
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
            if (HttpContext.Session.GetString("ErrorMessage") != null)
            {
                ViewBag.ErrorMessage = HttpContext.Session.GetString("ErrorMessage");
                HttpContext.Session.Remove("ErrorMessage");
            }

            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Include(r => r.IdCarNavigation.IdCarModelNavigation).Where(r => r.IdCarNavigation.IdCustomerNavigation.Name.Contains(search) || r.IdCarNavigation.IdCarModelNavigation.ModelName.Contains(search) || r.IdCarNavigation.LicensePlates.Contains(search));
                ViewData["GetTextSearch"] = search;
                return View(await garaContext.ToListAsync());
            }

            if (date != null)
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Include(r => r.IdCarNavigation.IdCarModelNavigation).Where(r => r.DateOfFactoryEntry == DateTime.Parse(date)).OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else if (state != null)
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Include(r => r.IdCarNavigation.IdCarModelNavigation).Where(r => r.State == state.Value).OrderByDescending(r => r.DateOfFactoryEntry);
                return View(await garaContext.ToListAsync());
            }
            else
            {
                var garaContext = _context.Repairs.Include(r => r.IdCarNavigation).ThenInclude(r => r.IdCustomerNavigation).Include(r => r.IdCarNavigation.IdCarModelNavigation).OrderByDescending(r => r.DateOfFactoryEntry);
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
                .Include(r => r.IdCarNavigation.IdCarModelNavigation)
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
                    var detailDelivery = repair.GoodsDeliveryNotes.Select(g => g.DetailGoodsDeliveryNotes).FirstOrDefault();
                    if (detailDelivery.Count() == 0)
                    {
                        ViewData["checkDetailGoodsDeliveryNotes"] = "Null";
                    }

                }

                return View(repair);
            }
        }

        // GET: Repairs/Create
        public IActionResult Create(int idCar, string layout = "_")
        {
            var existsRepair = _context.Repairs.Where(r => r.IdCar == idCar && r.State != StateType.completed).FirstOrDefault();
            if(existsRepair != null)
            {
                HttpContext.Session.SetString("ErrorMessage", "Xe này đang ở trong xưởng");
                return Json("false");
            }    
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["IdCar"] = idCar;
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
        public async Task<IActionResult> Edit(int? id, string layout = "_")
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
            ViewData["Layout"] = layout == "_" ? "" : layout;
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
            if(goodsDeliveryNote != null)
            {
                var detailGoodsDeliveryNote = _context.DetailGoodsDeliveryNotes.Include(dg => dg.IdGoodsDeliveryNoteNavigation).Where(dg => dg.IdGoodsDeliveryNote == goodsDeliveryNote.Id).ToList();
                if (detailGoodsDeliveryNote.Any())
                {
                    foreach (var itemdg in detailGoodsDeliveryNote)
                    {
                        _context.DetailGoodsDeliveryNotes.Remove(itemdg);
                    } 
                }
                _context.GoodsDeliveryNotes.Remove(goodsDeliveryNote);
            }
            var detailRepair = _context.DetailRepairs.Where(dr => dr.IdRepair == id).ToList();
            if (detailRepair.Any())
            {
                foreach (var itemdr in detailRepair)
                {
                    _context.DetailRepairs.Remove(itemdr);
                }
            }
           
            _context.Repairs.Remove(repair);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

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
                .Include(r => r.Pays)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (repair.Pays.Any())
            {
                ViewData["exists"] = "exists";
            }
            return View(repair);
        }
    }
}
