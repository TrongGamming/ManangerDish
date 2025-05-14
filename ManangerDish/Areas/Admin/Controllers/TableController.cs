using ManagerDish.Extensions;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Operations;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class TableController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public TableController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        [HttpGet]
        public async Task<IActionResult> Tables(string q = "", int pageNumber = 1, TableStatus? Status = null, int? Capacity = null)
        {
            const int pageSize = 5;
            var tables =  _unitOfWork.Table.GetAllQuery();
            if (!string.IsNullOrEmpty(q))
            {
                tables = tables.Where(x => x.Token.Contains(q) || x.Capicity.ToString().Contains(q));
            }
            if (Status != null)
            {
                tables = tables.Where(x => x.Status == Status);
            }
            if (Capacity != null)
            {
                tables = tables.Where(x => x.Capicity == Capacity);
            }
            var pagination = await tables.ToPageListAsync(t => t.Number, pageNumber, pageSize);
            ViewBag.Model = pagination;
            return View(pagination.Items);
        }

        public IActionResult CreateTable()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateTable(CreateTableRequest request)
        {
            if (ModelState.IsValid)
            {
                var table = new Table
                {
                    Capicity = request.Capicity,
                    Status = request.Status,
                    Token = request.Token
                };
                await _unitOfWork.Table.Create (table);
                await _unitOfWork.Save();
                return RedirectToAction("Tables");
            }
            return View(request);
        }
        [HttpGet]
        public async Task<IActionResult> EditTable(int id)
        {
            var table = await _unitOfWork.Table.Get(x => x.Number == id);
            if (table == null)
            {
                return NotFound();
            }
            var request = new EditTableRequest
            {
                Id = table.Number,
                Capicity = table.Capicity,
                Status = table.Status,
                Token = table.Token
            };
            return View(request);
        }

        [HttpPost]
        public async Task<IActionResult> EditTable(EditTableRequest request)
        {
            if (ModelState.IsValid)
            {
                var table = await _unitOfWork.Table.Get(x => x.Number == request.Id);
                if (table == null)
                {
                    return NotFound();
                }
                table.Capicity = request.Capicity;
                table.Status = request.Status;
                table.Token = request.Token;
                _unitOfWork.Table.Update(table);
                await _unitOfWork.Save();
                return RedirectToAction("Tables");
            }
            return View(request);
        }

        [HttpGet]
        public async Task<IActionResult> DeleteTable(int id)
        {
            var table = await _unitOfWork.Table.Get(x => x.Number == id);
            if (table == null)
            {
                return NotFound();
            }
            var request = new EditTableRequest
            {
                Id = table.Number,
                Capicity = table.Capicity,
                Status = table.Status,
                Token = table.Token
            };
            return View(request);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteTable(EditTableRequest request)
        {
            if (ModelState.IsValid)
            {
                var table = await _unitOfWork.Table.Get(x => x.Number == request.Id);
                if (table == null)
                {
                    return NotFound();
                }
                await _unitOfWork.Table.Remove(table);
                await _unitOfWork.Save();
                return RedirectToAction("Tables");
            }
            return View(request);
        }
    }
}
