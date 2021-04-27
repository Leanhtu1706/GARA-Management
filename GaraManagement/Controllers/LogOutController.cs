using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace GaraManagement.Controllers
{
    public class LogOutController : Controller
    {
        public IActionResult Index()
        {
            HttpContext.Session.Remove("SessionUserName");
            HttpContext.Session.Remove("PermissionAdmin");
            HttpContext.Session.Remove("PermissionCoVan");
            HttpContext.Session.Remove("PermissionThuKho");

            return RedirectToAction("Index", "Login");
        }
    }
}
