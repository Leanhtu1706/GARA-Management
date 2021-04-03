using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;

namespace GaraManagement.Controllers
{
    public class AccountsController : Controller
    {
        private readonly GaraContext _context;

        public AccountsController(GaraContext context)
        {
            _context = context;
        }

        // GET: Accounts
        public async Task<IActionResult> Index()
        {
            var garaContext = _context.Accounts.Include(a => a.IdEmployeeNavigation);
            return View(await garaContext.ToListAsync());
        }

        // GET: Accounts/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var account = await _context.Accounts
                .Include(a => a.IdEmployeeNavigation)
                .FirstOrDefaultAsync(m => m.UserName == id);
            if (account == null)
            {
                return NotFound();
            }

            return View(account);
        }

        // GET: Accounts/Create
        public IActionResult Create(string layout = "_")
        {
            ViewData["Layout"] = layout == "_" ? "" : layout;
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id");
            return View();
        }

        // POST: Accounts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Account account)
        {
            if (ModelState.IsValid)
            {
                account.Password = "25d55ad283aa400af464c76d713c07ad"; // default pass: 12345678
                _context.Add(account);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id", account.IdEmployee);
            return View(account);
        }

        // GET: Accounts/Edit/5
        public async Task<IActionResult> PhanQuyen(string username, string layout = "_")
        {
            if (username == null)
            {
                return NotFound();
            }
            ViewData["Layout"] = layout == "_" ? "" : layout;
            var account = await _context.Accounts.Include(a => a.Permissions).ThenInclude(a => a.IdPositionNavigation).Include(a => a.IdEmployeeNavigation).Where(a => a.UserName == username).FirstOrDefaultAsync();
            if (account == null)
            {
                return NotFound();
            }
            ViewData["IdEmployee"] = new SelectList(_context.Employees, "Id", "Id", account.IdEmployee);
            ViewData["Position"] = _context.Positions.Where(a => !_context.Permissions.Any(b => b.UserName == username && b.PositionId == a.Id)).ToList();
            return View(account);
        }

        // POST: Accounts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        public async Task<IActionResult> PhanQuyen(string username, List<int> listPosition)
        {
            if (username == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var permission_Old = await _context.Permissions.Where(a => a.UserName == username).ToListAsync();
                    var checkExist = 0;
                    foreach(var item in permission_Old)
                    {
                        foreach(var pos in listPosition)
                        {
                            if (item.PositionId == pos)
                            {
                                checkExist++;
                            }
                        }
                        
                        if(checkExist == 0)
                        {
                            _context.Permissions.Remove(item);
                        }
                        checkExist = 0;

                    }   
                    await _context.SaveChangesAsync();

                    var checkExist2 = 0;
                    var permissionAfterDelete = await _context.Permissions.Where(a => a.UserName == username).ToListAsync();
                    foreach (var item2 in listPosition)
                    {
                        foreach (var per in permissionAfterDelete)
                        {
                            if (item2 == per.PositionId)
                            {
                                checkExist2++;
                            }
                            
                        }
                        
                        if(checkExist2 == 0)
                        {
                            Permission permission_New = new Permission();
                            permission_New.UserName = username;
                            permission_New.PositionId = item2;
                            _context.Add(permission_New);
                        }
                        checkExist2 = 0;
                    }

                    
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AccountExists(username))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return Json(new { redirectToUrl = Url.Action("Index") });
            }
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetConfirmed(string username)
        {

            var account = _context.Accounts.Find(username);
            if (account == null)
            {
                return NotFound();
            }
            if (ModelState.IsValid)
            {
                try
                {
                    account.Password = "25d55ad283aa400af464c76d713c07ad"; // default pass: 12345678
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
                return RedirectToAction(nameof(Index));
            }
            return View(account);
        }
        

        // POST: Accounts/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string username)
        {

            var account = await _context.Accounts.FindAsync(username);
            var permission = await _context.Permissions.Where(a => a.UserName == username).ToListAsync();
            foreach(var item in permission)
            {
                _context.Permissions.Remove(item);
            }    
            if (account == null)
            {
                return NotFound();
            }
            _context.Accounts.Remove(account);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AccountExists(string username)
        {
            return _context.Accounts.Any(e => e.UserName == username);
        }
    }
}
