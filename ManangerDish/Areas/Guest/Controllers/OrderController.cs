using ManagerDish.Extensions;
using ManagerDish.Hubs;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.SignalR;

namespace ManagerDish.Areas.Guest.Controllers
{
    [Area("Guest")]
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
            var guest = await _unitOfWork.Guest.Get(g => g.Token == Request.Cookies["GuestToken"]);
            if (guest == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Guest" });
            }
            var orderdetails = _unitOfWork.OrderDetail.GetAllQuery();

            if (!string.IsNullOrEmpty(q))
            {
                orderdetails = orderdetails.Where(od => od.Guest.Name.Contains(q) || od.Dish.Name.Contains(q));
            }

            if (Status.HasValue)
            {
                orderdetails = orderdetails.Where(od => od.Status == Status);
            }
            var pagination = await orderdetails.ToPageListAsync(od => od.Id, pageNumber, DefaultPageSize);

            ViewBag.Model = pagination;
            ViewBag.search = q;

            ViewBag.TableId = guest.TableId;

            return View(pagination.Items);
        }


        [HttpGet]
        public async Task<IActionResult> Menu(int pageNumber, string q = "", string priceFilter = "", string categoryFilter = "")
        {
            int pageSize = 5;
            var guestToken = Request.Cookies["GuestToken"];
            var guest = await _unitOfWork.Guest.Get(g => g.Token == guestToken);
            if (guest == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Guest" });
            }
            var dishesQuery = _unitOfWork.Dish.GetAllQuery();
            if (!string.IsNullOrEmpty(q))
            {
                dishesQuery = dishesQuery.Where(d => d.Name.Contains(q) || d.Description.Contains(q));
            }

            if (!string.IsNullOrEmpty(priceFilter))
            {
                if (priceFilter == "asc")
                {
                    dishesQuery = dishesQuery.OrderBy(d => d.Price);
                }
                else if (priceFilter == "desc")
                {
                    dishesQuery = dishesQuery.OrderByDescending(d => d.Price);
                }
            }
            if (!string.IsNullOrEmpty(categoryFilter))
            {
                dishesQuery = dishesQuery.Where(d => d.Category.Id == int.Parse(categoryFilter));
            }
            var dishes = await dishesQuery.ToPageListAsync(d => d.Name, pageNumber, pageSize);

            var category = await _unitOfWork.Category.GetAllToListAsync();
            ViewBag.CategoriesSelect = category.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            ViewBag.Dishes = dishes.Items;
            ViewBag.Model = dishes;

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Menu(IEnumerable<GuestOrderItemDTO> GuestOrder)
        {
            var guestToken = Request.Cookies["GuestToken"];
            var guest = await _unitOfWork.Guest.Get(g => g.Token == guestToken);
            if (guest == null)
            {
                return RedirectToAction("Index", "Home", new { area = "Guest" });
            }
            if (GuestOrder == null || !GuestOrder.Any())
            {
                ModelState.AddModelError("", "No dishes selected.");
                return RedirectToAction("Menu");
            }

            var order = await _unitOfWork.Order.Get(o => o.TableId == guest.TableId && !o.Paid);

            foreach (var item in GuestOrder)
            {
                if (item.DishId != null && item.Quality > 0)
                {
                    var dish = await _unitOfWork.Dish.Get(d => d.Id == item.DishId);
                    if (dish != null)
                    {
                        var orderDetail = new OrderDetail
                        {
                            guestId = guest.Id,
                            DishId = dish.Id,
                            OrderId = order.Id,
                            Quantity = item.Quality,
                        };
                        await _unitOfWork.OrderDetail.Create(orderDetail);
                    }
                }
            }
            ViewBag.TableId = guest.TableId;
            await _unitOfWork.Save();
            await _hubContext.Clients.Group(order.TableId.ToString()).SendAsync("Refresh", "Refresh succes");
            return RedirectToAction("Orders");
        }

        [HttpPost]
        public IActionResult Logout()
        {
            Response.Cookies.Delete("GuestToken");
            return RedirectToAction("Index", "Home", new { area = "Guest" });
        }
    }
}
