using System.Threading.Tasks;
using ManagerDish.Extensions;
using ManagerDish.Helpers;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.VisualBasic;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class DishController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public DishController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region CRUD Dish
        [HttpGet]
        public async Task<IActionResult> Dishes(int pageNumber = 1,string q = "", int PriceFrom = 0, int PriceTo = int.MaxValue, int? CategoryId = null, DishStatus? Status = null)
        {
            int pagesize = 5;
            var dishes = _unitOfWork.Dish.GetAllQuery();
            if (!string.IsNullOrEmpty(q))
            {
               dishes = dishes.Where(d => d.Name.Contains(q) || d.Description.Contains(q));
            }
            dishes = dishes.Where(d => d.Price >= PriceFrom && d.Price <= PriceTo);
            if(CategoryId != null)
            {
                dishes = dishes.Where(d => d.CategoryId == CategoryId);
            }
            if(Status != null)
            {
                dishes = dishes.Where(d => d.Status == Status);
            }
            var pagination = await dishes.ToPageListAsync(d => d.CreatedAt, pageNumber, pagesize);
            ViewBag.Search = q;
            ViewBag.PriceFrom = PriceFrom;
            ViewBag.PriceTo = PriceTo;
            ViewBag.Model = pagination;
            var categories = await _unitOfWork.Category.GetAllToListAsync();
            ViewBag.categorySelect = categories.Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(pagination.Items);
        }

        [HttpGet]
        public async Task<IActionResult> DetailsDish(int id)
        {
            var dish = await _unitOfWork.Dish.Get(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }

        [HttpGet]   
        public  IActionResult CreateDish()
        {
            ViewBag.categorySelect =  _unitOfWork.Category.GetAllQuery().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();    
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateDish(CreateDishRequest request)
        {
            if (ModelState.IsValid)
            {
                var Image = await FileHelper.SaveImageAsync(request.Image, "dishes");
                if(Image == null)
                {
                    ModelState.AddModelError("Image", "Vui lòng chọn ảnh");
                    return View(request);
                }
                var dish = new Dish
                {
                    Name = request.Name,
                    Description = request.Description,
                    Price = request.Price,
                    Image = Image,
                    CategoryId = request.CategoryId,
                    Status = request.Status
                };
                await _unitOfWork.Dish.Create(dish);
                await _unitOfWork.Save();
                return RedirectToAction("Dishes");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditDish(int id)
        {
            var dish = await _unitOfWork.Dish.Get(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            var request = new CreateDishRequest
            {
                Name = dish.Name,
                Description = dish.Description,
                Price = dish.Price,
                ImagePath = dish.Image,
                CategoryId = dish.CategoryId ?? 0,
                Status = dish.Status
            };
            ViewBag.categorySelect = _unitOfWork.Category.GetAllQuery().Select(c => new SelectListItem
            {
                Value = c.Id.ToString(),
                Text = c.Name
            }).ToList();
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditDish(int Id, CreateDishRequest request)
        {
            if (ModelState.IsValid)
            {
                var dish = await _unitOfWork.Dish.Get(d => d.Id == Id);
                if (dish == null)
                {
                    return NotFound();
                }
                if (request.Image != null)
                {
                    var Image = await FileHelper.SaveImageAsync(request.Image, "dishes");
                    if (Image == null)
                    {
                        ModelState.AddModelError("Image", "Vui lòng chọn ảnh");
                        return View(request);
                    }
                    dish.Image = Image;
                }
                dish.Name = request.Name;
                dish.Description = request.Description;
                dish.Price = request.Price;
                dish.CategoryId = request.CategoryId;
                dish.Status = request.Status;
                _unitOfWork.Dish.Update(dish);
                await _unitOfWork.Save();
                return RedirectToAction("Dishes");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteDish(int id)
        {
            var dish = await _unitOfWork.Dish.Get(d => d.Id == id);
            if (dish == null)
            {
                return NotFound();
            }
            return View(dish);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteDish(int? Id)
        {
            var dish = await _unitOfWork.Dish.Get(d => d.Id == Id);
            if (dish == null)
            {
                return NotFound();
            }
            await _unitOfWork.Dish.Remove(dish);
            await _unitOfWork.Save();
            return RedirectToAction("Dishes");
        }
        #endregion
    }
}
