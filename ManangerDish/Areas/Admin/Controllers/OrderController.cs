using ManagerDish.Extensions;
using ManagerDish.Hubs;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using ManagerDish.Services;
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
        private readonly AuthService _authService;

        public OrderController(IUnitOfWork unitOfWork, IHubContext<OrderHub> hubContext, AuthService authService)
        {
            _unitOfWork = unitOfWork;
            _hubContext = hubContext;
            _authService = authService;
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
                    OrderId = o.Id,
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
                    Token = Guid.NewGuid().ToString("N"),
                };

                await _unitOfWork.Guest.Create(guest);
                await _unitOfWork.Save();
            }

            foreach (var item in request.GuestOrder)
            {
                if (item.Quality > 0)
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
            }

            await _hubContext.Clients.Group(guest.TableId.ToString()).SendAsync("Refresh", "Refresh Success");
            await _unitOfWork.Save();
            return RedirectToAction("Orders");
        }

        [HttpGet]
        public async Task<IActionResult> EditOrder(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.Get(od => od.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            ViewBag.OrderDetails = orderDetail;
            ViewBag.Dishes = await _unitOfWork.Dish.GetAllToListAsync();
            var editOrderRequest = new EditOrderRequest
            {
                OrderId = orderDetail.Id,
            };
            return View(editOrderRequest);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteOrder(int id)
        {
            var orderDetail = await _unitOfWork.OrderDetail.Get(od => od.Id == id);
            if (orderDetail == null)
            {
                return NotFound();
            }
            return View(orderDetail);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteOrder(OrderDetail orderDetail)
        {
            var existingOrderDetail = await _unitOfWork.OrderDetail.Get(od => od.Id == orderDetail.Id);
            if (existingOrderDetail == null)
            {
                return NotFound();
            }
            await _unitOfWork.OrderDetail.Remove(existingOrderDetail);
            await _unitOfWork.Save();
            return RedirectToAction("Orders");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangeStatus(int id, OrderStatus Status)
        {
            var referer = Request.Headers["Referer"].ToString();

            string localRedirectUrl;
            if (Uri.TryCreate(referer, UriKind.Absolute, out var refererUri))
            {
                localRedirectUrl = refererUri.PathAndQuery;
            }
            else
            {
                localRedirectUrl = referer;
            }
            var orderDetail = await _unitOfWork.OrderDetail.Get(od => od.Id == id);
            if (orderDetail == null)
                return NotFound();

            orderDetail.Status = Status;
            orderDetail.UpdatedAt = DateTime.UtcNow;
            var handler = await _authService.GetInFormationAccount();
            orderDetail.HandlerId = handler.Id;

            var order = await _unitOfWork.Order.Get(o => o.Id == orderDetail.OrderId);
            order.Total += orderDetail.Quantity * orderDetail.Dish.Price;

            _unitOfWork.OrderDetail.Update(orderDetail);
            _unitOfWork.Order.Update(order);
            await _unitOfWork.Save();
            await _hubContext.Clients.Group(orderDetail.Order.TableId.ToString()).SendAsync("Refresh", "Refresh succes");

            return LocalRedirect(localRedirectUrl);
        }

        [HttpGet]
        public async Task<IActionResult> Payment(int id)
        {
            var order = await _unitOfWork.Order.Get(o => o.Id == id && !o.Paid);
            if (order == null)
            {
                return NotFound();
            }
            var paymentRequest = new PaymentRequest
            {
                Order = order,
                TotalAmount = order.Total

            };
            return View(paymentRequest);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProcessPayment(ProcessPaymentRequest request)
        {
            if (!ModelState.IsValid)
            {
                return View(request);
            }
            var order = await _unitOfWork.Order.Get(o => o.Id == request.OrderId && !o.Paid);
            if (order == null)
            {
                return NotFound();
            }

            foreach(var guest in order.Guests)
            {
                guest.CheckOutTime = DateTime.UtcNow;
                _unitOfWork.Guest.Update(guest);
            }

            await _unitOfWork.Save();
            order.Paid = true;
            order.UpdatedAt = DateTime.UtcNow;
            _unitOfWork.Order.Update(order);
            await _unitOfWork.Save();
            await _hubContext.Clients.Group(order.TableId.ToString()).SendAsync("Refresh", "Refresh success");
            await _hubContext.Clients.Group(order.TableId.ToString()).SendAsync("Logout", "Logout success");
            return RedirectToAction("Orders");
        }
    }
}