using ManagerDish.Extensions;
using ManagerDish.Helpers;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Models.Enum;
using ManagerDish.Repository.IRepository;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.Operations;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class AccountController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        #region CRUD Account
        [HttpGet]
        public async Task<IActionResult> Accounts(string q,int pageNumber = 1, DateTime? InputDateTo = null, DateTime? InputDateFrom = null) 
        {
            int pageSize = 5;
            var accounts =  _unitOfWork.Account.GetAllQuery();
            accounts = accounts.AsQueryable();
            if (q != null && !string.IsNullOrEmpty(q))
            {
                accounts = accounts.Where(a => a.Name.Contains(q) || a.Email.Contains(q));
            }
            
            if (InputDateTo != null && InputDateFrom != null && InputDateTo != DateTime.MinValue && InputDateFrom != DateTime.MinValue)
            {
                accounts = accounts.Where(a => a.CreatedAt >= InputDateFrom && a.CreatedAt <= InputDateTo);
            }
            var pagedList = await accounts.ToPageListAsync(a => a.Name, pageNumber, pageSize);
            ViewBag.search = q;
            ViewBag.INputDateTo = InputDateTo;
            ViewBag.InputDateFrom = InputDateFrom;
            ViewBag.Model = pagedList;
            return View(pagedList.Items);
        }

        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateAccount(CreateAccountRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userIdClaim = User.FindFirst("sub")?.Value;
            if (string.IsNullOrEmpty(userIdClaim))
            {
                ModelState.AddModelError("", "User ID claim is missing.");
                return View(model);
            }

            var account = new Account
            {
                Name = model.UserName,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Email = model.Email,
                roleId = (RoleEnum)model.RoleId,
                CreatedBy = int.Parse(userIdClaim)
            };

            try
            {
                var avartarPath = await FileHelper.SaveImageAsync(model.Avatar, "avartars");
                account.Avatar = avartarPath;
            }
            catch(Exception ex)
            {
                account.Avatar = "/images/avartars/default-avatar.png";
            }

            await _unitOfWork.Account.Create(account);
            await _unitOfWork.Save();
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public async Task<IActionResult> EditAccount(int id)
        {
            var account = await _unitOfWork.Account.Get(x => x.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            var model = new EditAccountRequest
            {
                Id = account.Id,
                UserName = account.Name,
                Email = account.Email,
                RoleId = (int)account.roleId,
                ImagePath = account.Avatar,
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditAccount(EditAccountRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var account = await _unitOfWork.Account.Get(x => x.Id == model.Id);
            if (account == null)
            {
                return NotFound();
            }
            account.Name = model.UserName;
            account.Email = model.Email;
            account.roleId = (RoleEnum)model.RoleId;
            account.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            account.roleId = (RoleEnum)model.RoleId;

            if(!string.IsNullOrEmpty(account.Avatar) && account.Avatar != "/images/avartars/default-avatar.png")
            {
                var oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", account.Avatar.TrimStart('/').Replace("/", "\\"));

                if (System.IO.File.Exists(oldAvatarPath))
                {
                    System.IO.File.Delete(oldAvatarPath);
                }
            }

            var avartarPath = await FileHelper.SaveImageAsync(model.Avatar, "avartars");
            account.Avatar = avartarPath;
            
            _unitOfWork.Account.Update(account);
            await _unitOfWork.Save();
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            var account = await _unitOfWork.Account.Get(x => x.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            var hashValue = Guid.NewGuid().ToString();
            var model = new EditAccountRequest
            {
                Id = account.Id,
                UserName = account.Name,
                Email = account.Email,
                RoleId = (int)account.roleId,
                Password = hashValue,
                ConfirmPassword = hashValue,
                ImagePath = account.Avatar,
            };
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteAccount(EditAccountRequest model)
        {
            var account = await _unitOfWork.Account.Get(x => x.Id == model.Id);
            if (account == null)
            {
                return NotFound();
            }
            var oldAvatarPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", account.Avatar.TrimStart('/').Replace("/", "\\"));
            if (System.IO.File.Exists(oldAvatarPath))
            {
                System.IO.File.Delete(oldAvatarPath);
            }
            await _unitOfWork.Account.Remove(account);
            await _unitOfWork.Save();
            return RedirectToAction("Accounts");
        }

        [HttpGet]
        public async Task<IActionResult> DetailsAccount(int id)
        {
            var account = await _unitOfWork.Account.Get(x => x.Id == id);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }
        #endregion
    }
}
