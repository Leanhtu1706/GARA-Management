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
using System.Text;
using System.Security.Cryptography;

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
        // Convert text to MD5
        public static string MD5Hash(string input)
        {
            StringBuilder hash = new StringBuilder();
            MD5CryptoServiceProvider md5provider = new MD5CryptoServiceProvider();
            byte[] bytes = md5provider.ComputeHash(new UTF8Encoding().GetBytes(input));

            for (int i = 0; i < bytes.Length; i++)
            {
                hash.Append(bytes[i].ToString("x2"));
            }
            return hash.ToString();
        }
        public IActionResult Index()
        {
            var username = HttpContext.Session.GetString("SessionUserName");
            var password = HttpContext.Session.GetString("SessionPassword");

            if (username == null)
            {
                return RedirectToAction("Index", "Login");
            }
            if (HttpContext.Session.GetString("SuccessMessage") != null)
            {
                ViewBag.SuccessMessage = HttpContext.Session.GetString("SuccessMessage");
                HttpContext.Session.Remove("SuccessMessage");
            }
            var profile = _context.Accounts.Include(a => a.IdEmployeeNavigation).Where(a => a.UserName == username && a.Password == password).FirstOrDefault();

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
                    //-------------------------------------------
                    if (employee.ImageFile != null)
                    {
                        string wwwRootPath = _hostEnvironment.WebRootPath;
                        string fileName = Path.GetFileNameWithoutExtension(employee.ImageFile.FileName);
                        string extension = Path.GetExtension(employee.ImageFile.FileName);
                        fileName = fileName + extension;
                        employee.Image = "../assets/img/" + fileName;
                        var checkFile = Path.Combine(wwwRootPath + "/assets/img/", employee.ImageFile.FileName);
                        //Save image to wwwroot/image
                        if (!System.IO.File.Exists(checkFile))
                        {
                            string path = Path.Combine(wwwRootPath + "/assets/img/", fileName);
                            using (var fileStream = new FileStream(path, FileMode.Create))
                            {
                                await employee.ImageFile.CopyToAsync(fileStream);
                            }
                        }
                    }
                    //-------------------------------------------

                    _context.Update(employee);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

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

        public async Task<IActionResult> UpdatePassword(int? idEmployee, string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            if (idEmployee == null)
            {
                return NotFound();
            }
            else
            {
                var acc = await _context.Accounts.Where(a => a.IdEmployee == idEmployee).FirstOrDefaultAsync();
                if (acc != null)
                {
                    return View(acc);
                }
                return NotFound();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UpdatePassword(string userName, Account account)
        {
            if (userName != account.UserName)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(account);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(account.UserName))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return RedirectToAction("Index","Login");
            }
            return View(account);
        }
        private bool AccountExists(string userName)
        {
            return _context.Accounts.Any(e => e.UserName == userName);
        }        
        private bool EmployeeExists(int? id)
        {
            return _context.Employees.Any(e => e.Id == id);
        }


    }
}
