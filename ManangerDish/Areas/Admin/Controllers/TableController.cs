using Microsoft.AspNetCore.Mvc;

namespace ManagerDish.Areas.Admin.Controllers
{
    public class TableController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
