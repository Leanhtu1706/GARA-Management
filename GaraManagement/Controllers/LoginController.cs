using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Http;

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
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult Login(Account account)
        {
            var username = account.UserName;
            var password = account.Password;
            var loginResult =  _context.Accounts.Include(a=>a.IdEmployeeNavigation).Where(a=>(a.UserName == username && a.Password == password)).FirstOrDefault();
            if (loginResult != null)
            {
                HttpContext.Session.SetString("SessionUserName", loginResult.UserName);
                HttpContext.Session.SetString("SessionPassword", loginResult.Password);
                HttpContext.Session.SetString("SessionName",loginResult.IdEmployeeNavigation.Name);
                HttpContext.Session.SetString("SessionAvatar", loginResult.IdEmployeeNavigation.Image);
                return Json(new { redirectToUrl = Url.Action("Index", "Home")});

            }

            return Json("error");
        }

    }
}
