using ManagerDish.Extensions;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CategoryController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region CRUD Category
        [HttpGet]
        public async Task<IActionResult> Categories(string q = "", int pageNumber = 1)
        {
            int pageSize = 5;
            var categories = _unitOfWork.Category.GetAllQuery();
            if (!string.IsNullOrEmpty(q))
            {
                categories = categories.Where(c => c.Name.Contains(q) || c.Description.Contains(q));
            }
            var pagedList = await categories.ToPageListAsync(c => c.Name, pageNumber, pageSize);
            ViewBag.Search = q;
            ViewBag.Model = pagedList;
            return View(pagedList.Items);
        }
        [HttpGet]
        public IActionResult CreateCategory()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> DetailsCategory(int id)
        {
            var category = await _unitOfWork.Category.Get(c => c.Id == id);
            if(category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    Name = request.Name,
                    Description = request.Description,
                    CreatedAt = DateTime.Now
                };
                await _unitOfWork.Category.Create(category);
                await _unitOfWork.Save();
                TempData["success"] = "Thêm danh mục thành công";
                return RedirectToAction("Categories");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> EditCategory(int id)
        {
            var category = await _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            var model = new EditCategoryRequest
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditCategory(EditCategoryRequest request)
        {
            if (ModelState.IsValid)
            {
                var category = await _unitOfWork.Category.Get(c => c.Id == request.Id);
                if (category == null)
                {
                    return NotFound();
                }
                category.Name = request.Name;
                category.Description = request.Description;
                _unitOfWork.Category.Update(category);
                await _unitOfWork.Save();
                TempData["success"] = "Cập nhật danh mục thành công";
                return RedirectToAction("Categories");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCategory(int? id)
        {
            var category = await _unitOfWork.Category.Get(c => c.Id == id);
            if (category == null)
            {
                return NotFound();
            }
            _unitOfWork.Category.Remove(category);
            await _unitOfWork.Save();
            TempData["success"] = "Xóa danh mục thành công";
            return RedirectToAction("Categories");
        }
        #endregion
    }
}
