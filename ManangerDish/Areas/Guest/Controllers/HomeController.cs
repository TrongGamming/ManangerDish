using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using ManagerDish.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ManagerDish.Areas.Guest.Controllers
{
    [Area("Guest")]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            ViewBag.Name = User.Identity.Name;
            ViewBag.RoleName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;
            if (User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Accounts", "Account", new { area = "Admin" });
            }   
                return View();
        }
        [Authorize]
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
