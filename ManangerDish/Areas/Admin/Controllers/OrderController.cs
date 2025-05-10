using ManagerDish.Extensions;
using ManagerDish.Hubs;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;
using System.Linq;
using System.Threading.Tasks;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class OrderController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<OrderHub> _hubContext;
        private const int DefaultPageSize = 5;

        public OrderController(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
        }

        [HttpGet]
        public async Task<IActionResult> Orders(string q = "", int pageNumber = 1, OrderStatus? Status = null)
        {
            var orderdetails = _unitOfWork.OrderDetail.GetAllQuery();

            if (!string.IsNullOrEmpty(q))
            {
                orderdetails = orderdetails.Where(od => od.Guest.Name.Contains(q) || od.Dish.Name.Contains(q));
            }

            if (Status.HasValue)
            {
                orderdetails = orderdetails.Where(od => od.Status == Status);
            }

            var unpaidOrders = await _unitOfWork.Order.GetAllToListAsync(o => !o.Paid && o.Table.Status == TableStatus.Available);

            ViewBag.InforOrder = unpaidOrders.Select(o =>
            {
                var statusCounts = o.OrderDetails
                    .GroupBy(od => od.Status)
                    .ToDictionary(g => g.Key, g => g.Count());

                return new
                {
                    TableNumber = o.Table.Number,
                    QualityGuest = o.Guests.Count(),
                    PedingOrder = statusCounts.GetValueOrDefault(OrderStatus.Pending, 0),
                    ProcessingOrder = statusCounts.GetValueOrDefault(OrderStatus.Processing, 0),
                    CompletedOrder = statusCounts.GetValueOrDefault(OrderStatus.Completed, 0),
                    CancelledsOrder = statusCounts.GetValueOrDefault(OrderStatus.Cancelled, 0),
                };
            });

            var pagination = await orderdetails.ToPageListAsync(od => od.Id, pageNumber, DefaultPageSize);

            ViewBag.Model = pagination;
            ViewBag.search = q;

            var availableTables = await _unitOfWork.Table.GetAllToListAsync(t => t.Status == TableStatus.Available);
            ViewBag.ListTableId = availableTables.Select(t => t.Number).ToList();

            return View(pagination.Items);
        }

        [HttpGet]
        public async Task<IActionResult> CreateOrder()
        {
            var categories = await _unitOfWork.Category.GetAllToListAsync();
            var activeCustomers = await _unitOfWork.Guest.GetAllToListAsync(g => g.CheckOutTime > DateTime.UtcNow);
            var tables = await _unitOfWork.Table.GetAllToListAsync();
            var dishes = await _unitOfWork.Dish.GetAllToListAsync();

            ViewBag.CategoriesSelect = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();

            ViewBag.Dishes = dishes;

            ViewBag.CumstomerSelect = activeCustomers.Select(g => new SelectListItem
            {
                Value = g.Id.ToString(),
                Text = g.Name
            });

            ViewBag.TablesSelect = tables.Select(t => new SelectListItem
            {
                Value = t.Number.ToString(),
                Text = $"Table {t.Number}"
            });

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }

            var guest = await _unitOfWork.Guest.Get(g => g.Id == request.GuestId);

            var order = await _unitOfWork.Order.Get(o => o.TableId == request.TableId && o.Paid == false);

            if (order == null)
            {
                order = new Order
                {
                    TableId = request.TableId,
                    Total = 0,
                    Paid = false,
                };

                await _unitOfWork.Order.Create(order);
                await _unitOfWork.Save();
            }

            if (guest == null)
            {
                guest = new Models.Guest
                {
                    Name = request.NewGuestName,
                    PhoneNumber = request.NewGuestPhone,
                    TableId = request.TableId,
                    CreatedAt = DateTime.UtcNow,
                    CheckInTime = DateTime.UtcNow,
                    OrderId = order.Id,
                };

                await _unitOfWork.Guest.Create(guest);
                await _unitOfWork.Save();
            }

            foreach (var item in request.GuestOrder)
            {
                var orderDetail = new OrderDetail
                {
                    OrderId = guest.OrderId ?? order.Id, 
                    guestId = guest.Id,
                    DishId = item.DishId,
                    Quantity = item.Quality,
                    Status = OrderStatus.Pending,
                };

                await _unitOfWork.OrderDetail.Create(orderDetail);
            }

            await _hubContext.Clients.Group(guest.TableId.ToString()).SendAsync("Refresh", "Refresh Success");
            await _unitOfWork.Save();
            return RedirectToAction("Orders");
        }
    }
}