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
    public class LoginController : Controller
    {
        private readonly GaraContext _context;

        public LoginController(GaraContext context)
        {
            _context = context;
        }

        // GET: Login
        public IActionResult Login()
        {
            var loginResult = _context.Accounts.Include(a => a.IdEmployeeNavigation);
            //return View(await garaContext.ToListAsync());
            return View();
        }
        [HttpPost]
        public  IActionResult Login(Account account)
        {
            var username = account.UserName;
            var password = account.Password;
            var loginResult =  _context.Accounts.Include(a => a.IdEmployeeNavigation).Where(a=>(a.UserName == username && a.Password== password));
            if (loginResult.Any())
            {
                return Json("success");

            }

            return Json("error");
        }

    }
}
