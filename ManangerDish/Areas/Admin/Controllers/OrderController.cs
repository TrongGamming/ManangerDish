using System.Security.Cryptography.X509Certificates;
using ManagerDish.Extensions;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public OrderController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Orders(string q = "", int pageNumber = 1, OrderStatus? Status = null)
        {
            int pageSize = 5;
            var orderdetails = _unitOfWork.OrderDetail.GetAllQuery();
            if (!string.IsNullOrEmpty(q))
            {
                orderdetails.Where(od => od.Guest.Name.Contains(q) || od.Dish.Name.Contains(q));
            }
            if (Status != null)
            {
                orderdetails = orderdetails.Where(od => od.Status == Status);
            }
            var order = await _unitOfWork.Order.GetAllToListAsync(o => o.Paid == false && o.Table.Status == TableStatus.Available);
            ViewBag.InforOrder = order.Select(o =>
            {
                var groupedStatus = o.OrderDetails
                    .GroupBy(od => od.Status)
                    .ToDictionary(g => g.Key, g => g.Count());

                return new
                {
                    TableNumber = o.Table.Number,
                    QualityGuest = o.OrderDetails.Count(od => od.Guest != null),
                    PedingOrder = groupedStatus.GetValueOrDefault(OrderStatus.Pending, 0),
                    ProcessingOrder = groupedStatus.GetValueOrDefault(OrderStatus.Processing, 0),
                    CompletedOrder = groupedStatus.GetValueOrDefault(OrderStatus.Completed, 0),
                    CancelledsOrder = groupedStatus.GetValueOrDefault(OrderStatus.Cancelled, 0),
                };
            });
            var pagination = await orderdetails.ToPageListAsync(od => od.Id, pageNumber, pageSize);
            ViewBag.Model = pagination;
            ViewBag.search = q;
            return View(pagination.Items);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var categories = await _unitOfWork.Category.GetAllToListAsync();
            var cumstomer = await _unitOfWork.Guest.GetAllToListAsync(g => g.CheckOutTime < DateTime.UtcNow);
            var tables = await _unitOfWork.Table.GetAllToListAsync();


            ViewBag.CategoriesSelect = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Dishes = await _unitOfWork.Dish.GetAllToListAsync();

            ViewBag.CumstomerSelect = cumstomer.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            });

            ViewBag.TablesSelect = tables.Select(t => new SelectListItem
            {
                Value = t.Number.ToString(),
                Text = "Table " + t.Number
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            var accounts = await _unitOfWork.Account.GetAllToListAsync();
            return View();
        }
    }
}
