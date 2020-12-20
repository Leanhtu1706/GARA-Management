using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using X.PagedList;
using GaraManagement.ViewModels;
using Microsoft.AspNetCore.Http;

namespace GaraManagement.Controllers
{
    public class TypeOfSuppliesController : Controller
    {
        private readonly GaraContext _context;

        public TypeOfSuppliesController(GaraContext context)
        {
            _context = context;
        }

        // GET: TypeOfSupplies
        public IActionResult Index(int? pageNumber)
        {
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            if (pageNumber == null) pageNumber = 1;
            int pageSize = 10;
            return View( _context.TypeOfSupplies.ToList().ToPagedList((int)pageNumber, pageSize));
        }

        // GET: TypeOfSupplies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfSupply = await _context.TypeOfSupplies
                .FirstOrDefaultAsync(m => m.Id == id);
            if (typeOfSupply == null)
            {
                return NotFound();
            }

            return View(typeOfSupply);
        }

        // GET: TypeOfSupplies/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: TypeOfSupplies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create( TypeOfSupply typeOfSupply)
        {
            if (ModelState.IsValid)
            {
                typeOfSupply.CreateAt = DateTime.Now;
                _context.Add(typeOfSupply);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");
                return RedirectToAction(nameof(Index));
            }
            return View(typeOfSupply);
        }

        // GET: TypeOfSupplies/Edit/5
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var typeOfSupply = await _context.TypeOfSupplies.FindAsync(id);
            if (typeOfSupply == null)
            {
                return NotFound();
            }
            return View(typeOfSupply);
        }

        // POST: TypeOfSupplies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, TypeOfSupply typeOfSupply)
        {
            if (id != typeOfSupply.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    typeOfSupply.UpdateAt = DateTime.Now;
                    _context.Update(typeOfSupply);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TypeOfSupplyExists(typeOfSupply.Id))
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
            return View(typeOfSupply);
        }

        // GET: TypeOfSupplies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var typeOfSupply = await _context.TypeOfSupplies.FirstOrDefaultAsync(m => m.Id == id);
            var existSupplies = _context.Materials.Include(a => a.IdTypeNavigation).Where(a => a.IdType == id);
            var typeOfSupplyViewModel = new TypeOfSupplyViewModel();
            typeOfSupplyViewModel.material = existSupplies;
            typeOfSupplyViewModel.Id = typeOfSupply.Id;
            typeOfSupplyViewModel.Name = typeOfSupply.Name;
            typeOfSupplyViewModel.Description = typeOfSupply.Description;

            if (typeOfSupply == null)
            {
                return NotFound();
            }
            //if(existSupplies.Any())
            //{
            //    TempData["ErrorMessage"] = "Thực hiện xóa không thành công vì DANH MỤC đang tồn tại trong bảng VẬT TƯ!";
            //    return RedirectToAction(nameof(Index));
            //}

            return View(typeOfSupplyViewModel);
        }

        // POST: TypeOfSupplies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var typeOfSupply = await _context.TypeOfSupplies.FindAsync(id);
            var materials = _context.Materials.Where(a => a.IdType == id);
            _context.TypeOfSupplies.Remove(typeOfSupply);
            foreach(var item in materials)
            {
                _context.Materials.Remove(item);
            }    
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool TypeOfSupplyExists(int id)
        {
            return _context.TypeOfSupplies.Any(e => e.Id == id);
        }
    }
}
