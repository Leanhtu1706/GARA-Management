using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GaraManagement.Models;
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace GaraManagement.Controllers
{
    public class ProfileController : Controller
    {
        private readonly GaraContext _context;
        private readonly IWebHostEnvironment _hostEnvironment;

        public ProfileController(GaraContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("SessionUserName");
            var password = HttpContext.Session.GetString("SessionPassword");

            if (username == null)
            {
                return RedirectToAction("Index", "Login");
            }
            
            var profile =  _context.Accounts.Include(a=>a.IdEmployeeNavigation).Where(a=>a.UserName == username && a.Password == password).FirstOrDefault();
            
            ViewBag.image = profile.IdEmployeeNavigation.Image;
            if (profile == null)
            {
                return NotFound();
            }
            return View(profile.IdEmployeeNavigation);
        }

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
                return RedirectToAction(nameof(Index));
            }
            return View(employee);
        }
        private bool EmployeeExists(int? id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }
    }
}
