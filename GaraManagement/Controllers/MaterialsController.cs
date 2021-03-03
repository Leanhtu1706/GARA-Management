using System;
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
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;

namespace GaraManagement.Controllers
{
    public class MaterialsController : Controller
    {
        private readonly GaraContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public MaterialsController(GaraContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
        }

            // GET: Material
            //public  IActionResult Index(int? pageNumber)
            //{
            //    if (pageNumber == null) pageNumber = 1;
            //    int pageSize = 10;
            //    var garaContext = _context.Material.Include(s => s.IdTypeNavigation);

            //    return View(garaContext.ToList().ToPagedList((int)pageNumber,pageSize));
            //}
            [HttpGet]
        public IActionResult Index(string search)
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

            ViewData["GetTextSearch"] = search;
            
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Materials.Include(s => s.IdTypeNavigation).Include(s => s.PriceMaterials).Include(s => s.IdCarModelNavigation).Where(a => a.Name.Contains(search));
                return View(garaContext.ToList());
            }
            else
            {
                var garaContext = _context.Materials.Include(s => s.IdTypeNavigation).Include(s => s.PriceMaterials).Include(s =>s.IdCarModelNavigation);
                return View(garaContext.ToList());
            }
            
        }
        // GET: Material/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supply = await _context.Materials
                .Include(s => s.IdTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // GET: Material/Create
        [HttpGet]
        public IActionResult Create(string layout = "_")
        {
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            ViewData["CarName"] = new SelectList(_context.CarModels, "Id", "ModelName");
            
            return View();
        }

        // POST: Material/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Material material)
        {
            var type = _context.TypeOfSupplies.Select(i => i.Name).ToList();
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                if (material.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(material.ImageFile.FileName);
                    string extension = Path.GetExtension(material.ImageFile.FileName);
                    fileName = fileName + extension;
                    material.Image = "../assets/img/" + fileName;
                    var checkFile = Path.Combine(wwwRootPath + "/assets/img/", material.ImageFile.FileName);
                    //Save image to wwwroot/image
                    if (!System.IO.File.Exists(checkFile))
                    {
                        string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await material.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                }

                material.CreateAt = DateTime.Now;
                _context.Add(material);
                PriceMaterial priceMaterial = new PriceMaterial();
                await _context.SaveChangesAsync();
                priceMaterial.IdMaterial = material.Id;
                priceMaterial.UpdateAt = DateTime.Now;
                priceMaterial.Price = material.Price;
                _context.Add(priceMaterial);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            return View(material);
        }

        // GET: Material/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            
            if (id == null)
            {
                return NotFound();
            }           
            var type =  _context.TypeOfSupplies.Select(i => i.Name).ToList();
            var image = _context.Materials.Where(a => a.Id == id).Select(i => i.Image).FirstOrDefault();
            ViewBag.image = image;
            var material = await _context.Materials.FindAsync(id);
            var priceMaterial = await _context.PriceMaterials.Where(p => p.IdMaterial == material.Id).OrderByDescending(p => p.UpdateAt).FirstAsync();
            material.Price = priceMaterial.Price;
            if (material == null)
            {
                return NotFound();
            }
            ViewData["TypeName"] = new SelectList(_context.TypeOfSupplies, "Id", "Name", type);
            ViewData["CarName"] = new SelectList(_context.CarModels, "Id", "ModelName");
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View(material);
        }

        // POST: Material/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Material material)
        {
            if (id != material.Id)
            {
                return NotFound();
            }
         
            if (ModelState.IsValid)
            {
                try
                {
                    if(material.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(material.ImageFile.FileName);
                        string extension = Path.GetExtension(material.ImageFile.FileName);
                        fileName = fileName + extension;
                        material.Image = "../assets/img/" + fileName;
                        var checkFile = Path.Combine(wwwRootPath + "/assets/img/", material.ImageFile.FileName);
                        //Save image to wwwroot/image
                        if (!System.IO.File.Exists(checkFile))
                        {
                            string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await material.ImageFile.CopyToAsync(fileStream);
                            }
                        }
                    }    

                    material.UpdateAt = DateTime.Now;       
                    _context.Update(material);
                    await _context.SaveChangesAsync();
                    var priceCheck = _context.PriceMaterials.Where(p => p.IdMaterial == material.Id).OrderByDescending(p => p.UpdateAt).First().Price;
                    if(priceCheck != material.Price)
                    {
                        PriceMaterial priceMaterial = new PriceMaterial();
                        priceMaterial.IdMaterial = material.Id;
                        priceMaterial.UpdateAt = DateTime.Now;
                        priceMaterial.Price = material.Price;
                        _context.Add(priceMaterial);
                        await _context.SaveChangesAsync();
                    }
                    
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SupplyExists(material.Id))
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
            return View(material);
        }

        // GET: Material/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var supply = await _context.Materials
                .Include(s => s.IdTypeNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (supply == null)
            {
                return NotFound();
            }

            return View(supply);
        }

        // POST: Material/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var supply = await _context.Materials.FindAsync(id);
            var detailGoodDelivery = _context.DetailGoodsDeliveryNotes.Where(d => d.IdMaterial == id);
            var detailGoodReceived = _context.DetailGoodsReceivedNotes.Where(d => d.IdMaterial == id);
            foreach(var delivery in detailGoodDelivery)
            {
                _context.DetailGoodsDeliveryNotes.Remove(delivery);
            }            
            foreach(var received in detailGoodReceived)
            {
                _context.DetailGoodsReceivedNotes.Remove(received);
            }
            _context.Materials.Remove(supply);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool SupplyExists(int? id)
        {
            return _context.Materials.Any(e => e.Id == id);
        }

        public IActionResult GetAmount(int id)
        {
            var amount =  _context.Materials.Where(m=>m.Id == id).Select(m=>m.Amount);
            return Json(amount);

        }
    }
}
