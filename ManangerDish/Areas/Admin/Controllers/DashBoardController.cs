using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DashBoardController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DashBoardController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult GetFavoriteDishes(string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out var start))
                return BadRequest(new { message = "startDate không hợp lệ" });
            if (!DateTime.TryParse(endDate, out var end))
                return BadRequest(new { message = "endDate không hợp lệ" });

            end = end.Date.AddDays(1).AddTicks(-1);

            var result = _unitOfWork.OrderDetail
                .GetAllQuery()
                .Where(od => od.Order.CreatedAt >= start && od.Order.CreatedAt <= end)
                .GroupBy(od => od.Dish.Name)
                .Select(g => new
                {
                    DishName = g.Key,
                    OrderCount = g.Sum(x => x.Quantity)
                })
                .OrderByDescending(x => x.OrderCount)
                .Take(5)
                .ToList();

            return Json(result);
        }

        [HttpGet]
        public IActionResult GetRevenueData(string startDate, string endDate)
        {
            if (!DateTime.TryParse(startDate, out var start))
                return BadRequest(new { message = "startDate không hợp lệ" });
            if (!DateTime.TryParse(endDate, out var end))
                return BadRequest(new { message = "endDate không hợp lệ" });

            end = end.Date.AddDays(1).AddTicks(-1);

            // Daily revenue
            var orders = _unitOfWork.Order
                .GetAllQuery()
                .Where(o => o.CreatedAt >= start && o.CreatedAt <= end)
                .ToList();

            var dailyRevenue = orders
                .GroupBy(o => o.CreatedAt.Date)
                .Select(g => new
                {
                    Date = g.Key,
                    Revenue = g.Sum(o => o.Total)
                })
                .OrderBy(g => g.Date)
                .ToList();

            var totalOrders = orders.Count;
            var totalRevenue = orders.Sum(o => o.Total);
            var avgOrderValue = totalOrders == 0 ? 0 : totalRevenue / totalOrders;

            return Json(new
            {
                dailyRevenue = dailyRevenue.Select(x => new
                {
                    date = x.Date.ToString("yyyy-MM-dd"),
                    revenue = x.Revenue
                }),
                overview = new
                {
                    totalOrders,
                    totalRevenue,
                    avgOrderValue
                }
            });
        }

    }
}
