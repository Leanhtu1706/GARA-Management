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
    public class HomeController : Controller
    {
        private readonly GaraContext _context;

        public HomeController(GaraContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("SessionUserName") == null)
            {
                return RedirectToAction("Index", "Login");
            }
            else
            {
                return View();
            }
            
        }

    }
}
