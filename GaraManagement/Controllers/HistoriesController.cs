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
    public class HistoriesController : Controller
    {
        private readonly GaraContext _context;

        public HistoriesController(GaraContext context)
        {
            _context = context;
        }

        // GET: Histories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("SessionUserName") == null || HttpContext.Session.GetString("PermissionAdmin") != "Yes")
            {
                return RedirectToAction("Index", "Login");
            }
            var garaContext = _context.Historys.Include(h => h.UserNameNavigation);
            return View(await garaContext.ToListAsync());
        }
        
    }
}
