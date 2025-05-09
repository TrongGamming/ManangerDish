using ManagerDish.Models.DTO;
using ManagerDish.Repository.IRepository;
using ManagerDish.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ManagerDish.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IConfiguration _configuration;

        public AuthController(AuthService authService, IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _authService = authService;
            _unitOfWork = unitOfWork;
            _configuration = configuration;
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginRequest model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var account = await _authService.AuthenticateAccount(model.UserName, model.Password);
            if (account == null){
                ModelState.AddModelError(string.Empty,"Tài khoản hoặc mật khẩu không hợp lệ");
                return View(model);
            }
            var token = _authService.GenerateTokenResponse(account);
            var refreshTokenExpirationDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpirationDays"]);
            Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
            {
                HttpOnly = true,
                Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays),
                SameSite = SameSiteMode.None,
                Secure = true
            });
            return RedirectToAction("Index", "Home", new { area = "Guest" });

        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> RefreshToken()
        {
            var refreshToken = await _unitOfWork.RefreshToken.Get(rf => rf.Token == Request.Cookies["refreshToken"]);
            if(refreshToken == null || string.IsNullOrEmpty(refreshToken.AccessToken))
            {
                return Unauthorized("Refresh token không hợp lệ");
            }
            try
            {
                var token = await _authService.ValidateAndRefreshTokens(refreshToken.AccessToken, refreshToken.Token);
                refreshToken.AccessToken = token.AccessToken;
                _unitOfWork.RefreshToken.Update(refreshToken);
                await _unitOfWork.Save();
                var refreshTokenExpirationDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpirationDays"]);
                Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                {
                    HttpOnly = true,
                    Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays),
                    SameSite = SameSiteMode.None,
                    Secure = true
                });
                return Ok(new
                {
                    accessToken = token.AccessToken
                });

            }
            catch (UnauthorizedAccessException ex)
            {
                return RedirectToAction("logout");
            }
            catch (SecurityTokenException e)
            {
                return RedirectToAction("logout");

            }

        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            var refreshToken = await _unitOfWork.RefreshToken.Get(rf => rf.Token == Request.Cookies["refreshToken"]);
            await _unitOfWork.RefreshToken.Remove(refreshToken);
            await _unitOfWork.Save();
            Response.Cookies.Delete("refreshToken");
            return RedirectToAction("Index", "Home", new { area = "Guest" });
        }

    }
}