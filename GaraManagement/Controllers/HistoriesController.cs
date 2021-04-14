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
            var garaContext = _context.Historys.Include(h => h.UserNameNavigation);
            return View(await garaContext.ToListAsync());
        }
        
    }
}
