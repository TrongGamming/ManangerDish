using Microsoft.AspNetCore.Mvc;

namespace ManagerDish.Areas.Admin.Controllers
{
    public class DashBoardController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
