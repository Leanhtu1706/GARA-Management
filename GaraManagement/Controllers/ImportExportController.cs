using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace GaraManagement.Controllers
{
    public class ImportExportController : Controller
    {
        public IActionResult _ImportPartialView()
        {
            return View();
        }        
        public IActionResult _ExportPartialView()
        {
            return View();
        }
    }
}
