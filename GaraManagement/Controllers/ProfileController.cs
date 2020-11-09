using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using GaraManagement.Models;

namespace GaraManagement.Controllers
{
    public class ProfileController : Controller
    {
        private readonly GaraContext _context;

        public ProfileController(GaraContext context)
        {
            _context = context;
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
