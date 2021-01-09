﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;
using Microsoft.AspNetCore.Http;


namespace GaraManagement.Controllers
{
    public class CarsController : Controller
    {
        private readonly GaraContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;
        public CarsController(GaraContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        // GET: Cars
        public async Task<IActionResult> Index(string search, int? idCustomer)
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

            ViewData["GetTextSearch"] = search; // vẫn hiển thị tên search khi load lại index
            if (idCustomer != null)
            {
                ViewBag.IdCustomer = idCustomer;
                ViewBag.CustomerName = _context.Customers.Where(c => c.Id == idCustomer).Select(c=>c.Name).FirstOrDefault();
                var carData = _context.Cars.Include(i => i.IdCustomerNavigation).Where(a => a.IdCustomerNavigation.Id == idCustomer);
                return View(carData.ToList());
            }    
            
            if (!string.IsNullOrEmpty(search))
            {
                var carData = _context.Cars.Include(i => i.IdCustomerNavigation).Where(a => a.CarName.Contains(search) || a.Manufacturer.Contains(search) || a.IdCustomerNavigation.Name.Contains(search));
                return View(carData.ToList());
            }
            else
            {
                //Tí nhớ xóa 2 dòng này nha
                //var customer = _context.Customers.Select(i => i.Name).ToList();
                //ViewData["Customer"] = new SelectList(_context.Customers, "Id", "Name", customer);
                var carData = _context.Cars.Include(c => c.IdCustomerNavigation);
                return View(await carData.ToListAsync());
            }
        }

        // GET: Cars/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.IdCustomerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }

            return View(car);
        }

        // GET: Cars/Create
        public IActionResult Create(int? idCustomer, string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            if (idCustomer != null)
            {
                ViewData["Customer"] = new SelectList(_context.Customers.Where(c=>c.Id == idCustomer), "Id", "Name");
            }
            else
            {
                ViewData["Customer"] = new SelectList(_context.Customers, "Id", "Name");
            }
            return View();
        }

        // POST: Cars/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Car car)
        {
            if (ModelState.IsValid)
            {
                //-------------------------------------------
                if (car.ImageFile != null)
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                    string extension = Path.GetExtension(car.ImageFile.FileName);
                    fileName = fileName + extension;
                    car.Image = "../assets/img/" + fileName;
                    var checkFile = Path.Combine(wwwRootPath + "/assets/img/", car.ImageFile.FileName);
                    //Save image to wwwroot/image
                    if (!System.IO.File.Exists(checkFile))
                    {
                        string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await car.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                }
                //-------------------------------------------

                _context.Add(car);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                return RedirectToAction(nameof(Index));
            }
           
            ViewData["Customer"] = new SelectList(_context.Customers, "Id", "Id", car.IdCustomer);
            return View(car);
        }

        // GET: Cars/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            var customer = _context.Customers.Select(i => i.Name).ToList();
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            ViewBag.image = car.Image;
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["Customer"] = new SelectList(_context.Customers, "Id", "Name", car.IdCustomer);
            return View(car);
        }

        // POST: Cars/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Car car)
        {
            if (id != car.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //-------------------------------------------
                    if (car.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(car.ImageFile.FileName);
                        string extension = Path.GetExtension(car.ImageFile.FileName);
                        fileName = fileName + extension;
                        car.Image = "../assets/img/" + fileName;
                        var checkFile = Path.Combine(wwwRootPath + "/assets/img/", car.ImageFile.FileName);
                        //Save image to wwwroot/image
                        if (!System.IO.File.Exists(checkFile))
                        {
                            string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await car.ImageFile.CopyToAsync(fileStream);
                            }
                        }
                    }
                    //-------------------------------------------

                    _context.Update(car);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CarExists(car.Id))
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
            ViewData["IdCustomer"] = new SelectList(_context.Customers, "Id", "Id", car.IdCustomer);
            return View(car);
        }

        // GET: Cars/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var car = await _context.Cars
                .Include(c => c.IdCustomerNavigation)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (car == null)
            {
                return NotFound();
            }
            ViewBag.idDelete = id;
            return Json(car.Id);
        }

        // POST: Cars/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var car = await _context.Cars.FindAsync(id);
            _context.Cars.Remove(car);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            return RedirectToAction(nameof(Index));
        }

        private bool CarExists(int id)
        {
            return _context.Cars.Any(e => e.Id == id);
        }
    }
}
