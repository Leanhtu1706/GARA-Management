using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using GaraManagement.Models;
using Microsoft.AspNetCore.Http;
using System.Text;
using System.Security.Cryptography;

namespace GaraManagement.Controllers
{
    public class LoginController : Controller
    {
        private readonly GaraContext _context;

        public LoginController(GaraContext context)
        {
            _context = context;
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
            var loginResult =  _context.Accounts.Include(a=>a.IdEmployeeNavigation).Where(a=>(a.UserName == username && a.Password == MD5Hash(password))).FirstOrDefault();
            if (loginResult != null)
            {
                HttpContext.Session.SetString("SessionUserName", loginResult.UserName);
                HttpContext.Session.SetString("SessionPassword", loginResult.Password);
                HttpContext.Session.SetString("SessionName", loginResult.IdEmployeeNavigation.Name);
                HttpContext.Session.SetString("SessionAvatar", loginResult.IdEmployeeNavigation.Image);
                return Json(new { redirectToUrl = Url.Action("Index", "Home") });

            }
            else
            {
                return Json("error");
            }
        }

    }
}
