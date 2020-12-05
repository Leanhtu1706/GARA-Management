using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using X.PagedList;
using Microsoft.Extensions.Hosting;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace GaraManagement.Controllers
{
    public class EmployeesController : Controller
    {
        private readonly GaraContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public EmployeesController(GaraContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }

        [HttpGet]
        public IActionResult Index(string search)
        {

            ViewData["GetTextSearch"] = search;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Employees.Where(a => a.Name.Contains(search));
                return View(garaContext.ToList());
            }
            else
            {
                var garaContext = _context.Employees;
                return View(garaContext.ToList());
            }

        }
        // GET: Employees/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // GET: Employees/Create
        [HttpGet]
        public IActionResult Create(string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: Employees/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Employee employee)
        {
            //-------------------------------------------
            if (ModelState.IsValid)
            {
                //Save image to wwwroot/image
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                string extension = Path.GetExtension(employee.ImageFile.FileName);
                employee.Image = "../assets/img/" + fileName + extension;
                var checkFile = @"D:\Tài liệu\Đồ án Chuyên ngành\Gara clone\GaraManagement\wwwroot\assets\img\" + employee.ImageFile.FileName;
                //Save image to wwwroot/image
                if (!System.IO.File.Exists(checkFile))
                {
                    fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                    employee.Image = "../assets/img/" + fileName;
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await employee.ImageFile.CopyToAsync(fileStream);
                    }
                }
                //-------------------------------------------

                _context.Add(employee);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Thêm mới thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Edit/5
        [HttpGet]
        public async Task<IActionResult> Edit(int? id, string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var employee = await _context.Employees.FindAsync(id);
            var image = _context.Employees.Where(a => a.Id == id).Select(i => i.Image).FirstOrDefault();
            ViewBag.image = image;
            if (employee == null)
            {
                return NotFound();
            }
            return View(employee);
        }

        // POST: Employees/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Employee employee)
        {
            if (id != employee.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                    string extension = Path.GetExtension(employee.ImageFile.FileName);
                    employee.Image = "../assets/img/" + fileName + extension;
                    var checkFile = @"D:\Tài liệu\Đồ án Chuyên ngành\Gara clone\GaraManagement\wwwroot\assets\img\" + employee.ImageFile.FileName;
                    //Save image to wwwroot/image
                    if (!System.IO.File.Exists(checkFile))
                    {                       
                        fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                        string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                        employee.Image = "../assets/img/" + fileName;
                        using (var fileStream = new FileStream(path, FileMode.Create))
                        {
                            await employee.ImageFile.CopyToAsync(fileStream);
                        }
                    }
                    //===============================
                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EmployeeExists(employee.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                TempData["SuccessMessage"] = "Sửa thành công!";
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }

        // GET: Employees/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var employee = await _context.Employees
                .FirstOrDefaultAsync(m => m.Id == id);
            if (employee == null)
            {
                return NotFound();
            }

            return View(employee);
        }

        // POST: Employees/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var employee = await _context.Employees.FindAsync(id);
            _context.Employees.Remove(employee);
            await _context.SaveChangesAsync();
            TempData["SuccessMessage"] = "Xóa thành công!";
            return RedirectToAction(nameof(Index));
        }

        private bool EmployeeExists(int? id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
