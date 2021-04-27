using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using X.PagedList;
using Microsoft.AspNetCore.Http;

namespace GaraManagement.Controllers
{
    public class CustomersController : Controller
    {
        private readonly GaraContext _context;

        public CustomersController(GaraContext context)
        {
            _context = context;
        }

        [HttpGet]
        public  IActionResult Index(string search)
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
            ViewData["GetTextSearch"] = search;
            if (!string.IsNullOrEmpty(search))
            {
                var garaContext = _context.Customers.Where(a => a.Name.Contains(search) || a.IdentityCardNumber == search || a.Phone == search);
                return View(garaContext.ToList());
            }
            else
            {
                var garaContext = _context.Customers;
                return View(garaContext.ToList());
            }           
        }

        // GET: Customers/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // GET: Customers/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            return View();
        }

        // POST: Customers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Customer customer)
        {
            if (ModelState.IsValid)
            {
                _context.Add(customer);
                await _context.SaveChangesAsync();
                HttpContext.Session.SetString("SuccessMessage", "Thêm mới thành công");

                //History
                History history = new History();
                history.DateHistory = DateTime.Now;
                history.UserName = HttpContext.Session.GetString("SessionUserName");
                history.Event = "Thêm khách hàng " + customer.Name;
                _context.Add(history);
                await _context.SaveChangesAsync();
                //============================
                return RedirectToAction(nameof(Index));
            }           
            return View(customer);
        }

        // GET: Customers/Edit/5
        public async Task<IActionResult> Edit(int? id,string layout = "_")
        {
            if (id == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // POST: Customers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int? id, Customer customer)
        {
            if (id != customer.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(customer);
                    await _context.SaveChangesAsync();
                    HttpContext.Session.SetString("SuccessMessage", "Cập nhật thành công");

                    //History
                    History history = new History();
                    history.DateHistory = DateTime.Now;
                    history.UserName = HttpContext.Session.GetString("SessionUserName");
                    history.Event = "Cập nhật thông tin khách hàng " + customer.Name;
                    _context.Add(history);
                    await _context.SaveChangesAsync();
                    //============================

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CustomerExists(customer.Id))
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
            return View(customer);
        }

        // GET: Customers/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers
                .FirstOrDefaultAsync(m => m.Id == id);
            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        // POST: Customers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var customer = await _context.Customers.FindAsync(id);
            _context.Customers.Remove(customer);
            await _context.SaveChangesAsync();
            HttpContext.Session.SetString("SuccessMessage", "Xóa thành công");

            //History
            History history = new History();
            history.DateHistory = DateTime.Now;
            history.UserName = HttpContext.Session.GetString("SessionUserName");
            history.Event = "Xóa khách hàng " + customer.Name;
            _context.Add(history);
            await _context.SaveChangesAsync();
            //============================
            return RedirectToAction(nameof(Index));
        }

        private bool CustomerExists(int? id)
        {
            return _context.Customers.Any(e => e.Id == id);
        }
    }
}
