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
    public class PaysController : Controller
    {
        private readonly GaraContext _context;

        public PaysController(GaraContext context)
        {
            _context = context;
        }

        // GET: Pays
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("SessionUserName") == null || HttpContext.Session.GetString("PermissionAdmin") != "Yes")
            {
                if (HttpContext.Session.GetString("PermissionCoVan") != "Yes")
                {
                    return RedirectToAction("Index", "Login");
                }
            }

            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            var garaContext = _context.Pays.Include(p => p.IdRepairNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: Pays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .Include(r => r.IdRepairNavigation.GoodsDeliveryNotes)
                .ThenInclude(r => r.DetailGoodsDeliveryNotes)
                .ThenInclude(r => r.IdMaterialNavigation)
                .ThenInclude(r => r.PriceMaterials)
                .Include(r => r.IdRepairNavigation.DetailRepairs)
                .ThenInclude(r => r.IdWorkNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation.IdCarModelNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return View(pay);
        }

        // GET: Pays/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id","Id");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: Pays/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( Pay pay)
        {
            if (ModelState.IsValid)
            {   
                int ? workCost = 0;
                var workCostData = await _context.DetailRepairs.Include(r => r.IdWorkNavigation).Where(r =>r.IdRepair == pay.IdRepair).ToListAsync();
                if (workCostData.Any())
                {

                    foreach (var item in workCostData)
                    {
                        workCost += item.IdWorkNavigation.Cost * item.Amount;

                    }

                }
                int ? materialCost = 0;
                var materialCostData = await _context.GoodsDeliveryNotes.Include(r => r.DetailGoodsDeliveryNotes).Where(r =>r.IdRepair == pay.IdRepair).Select(r => r.DetailGoodsDeliveryNotes).FirstAsync();
                if (materialCostData.Any())
                {

                    foreach (var item in materialCostData)
                    {
                        materialCost += item.Price * item.Amount;

                    }

                }
                pay.Total = workCost + materialCost;
                _context.Add(pay);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới biên lai thành công");

                return RedirectToAction(nameof(Index));
            }
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Edit/5
        public async Task<IActionResult> Edit(int? idRepair, string layout = "_")
        {
            if (idRepair == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays.Where(p => p.IdRepair == idRepair).FirstOrDefaultAsync();
            if (pay == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(pay);
        }

        // POST: Pays/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Pay pay)
        {
            if (id != pay.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    pay.Paid += pay.owe;
                    pay.Update_at = DateTime.Now;
                    _context.Update(pay);
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!PayExists(pay.Id))
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
            ViewData["IdRepair"] = new SelectList(_context.Repairs, "Id", "Id", pay.IdRepair);
            return View(pay);
        }

        // GET: Pays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pay = await _context.Pays
                .Include(p => p.IdRepairNavigation)
                .Include(r => r.IdRepairNavigation.IdCarNavigation)
                .ThenInclude(r => r.IdCustomerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pay == null)
            {
                return NotFound();
            }

            return Json(id);
            
        }

        // POST: Pays/Delete/5
        [HttpPost]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var pay = await _context.Pays.FindAsync(id);
            _context.Pays.Remove(pay);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool PayExists(int id)
        {
            return _context.Pays.Any(e => e.Id == id);
        }
    }
}
