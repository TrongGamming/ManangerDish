using System.IdentityModel.Tokens.Jwt;
using System.Text.Json;
using System.Threading.Tasks;
using ManagerDish.Repository.IRepository;
using ManagerDish.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing.Tree;
using Microsoft.IdentityModel.Tokens;

namespace ManagerDish.Middleware
{
    public class AutomaticRefreshToken : IMiddleware
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly AuthService _authService;

        public AutomaticRefreshToken( IUnitOfWork unitOfWork, IConfiguration configuration, AuthService authService)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _authService = authService;
        }
        public async Task InvokeAsync(HttpContext context, RequestDelegate next)
        {
            var cookieToken = context.Request.Cookies["refreshToken"];
            if (string.IsNullOrEmpty(cookieToken))
            {
                await next(context);
                return;
            }

            var refreshToken = await _unitOfWork.RefreshToken.Get(rf => rf.Token == cookieToken);
            if (refreshToken == null || string.IsNullOrEmpty(refreshToken.AccessToken))
            {
                await next(context);
                return;
            }

            var accessToken = refreshToken.AccessToken;
            context.Request.Headers["Authorization"] = $"Bearer {accessToken}";

            if (IsTokenExpired(accessToken))
            {
                try
                {
                    var token = await _authService.ValidateAndRefreshTokens(refreshToken.AccessToken, refreshToken.Token);

                    refreshToken.AccessToken = token.AccessToken;
                    _unitOfWork.RefreshToken.Update(refreshToken);
                    await _unitOfWork.Save();

                    var refreshTokenExpirationDays = Convert.ToInt32(_configuration["Jwt:RefreshTokenExpirationDays"]);
                    context.Response.Cookies.Append("refreshToken", token.RefreshToken, new CookieOptions
                    {
                        HttpOnly = true,
                        Expires = DateTimeOffset.UtcNow.AddDays(refreshTokenExpirationDays),
                        SameSite = SameSiteMode.None,
                        Secure = true
                    });

                    context.Request.Headers["Authorization"] = $"Bearer {token.AccessToken}";
                }
                catch (UnauthorizedAccessException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await _unitOfWork.RefreshToken.Remove(refreshToken);
                    await _unitOfWork.Save();
                    return;
                }
                catch (SecurityTokenException)
                {
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    await _unitOfWork.RefreshToken.Remove(refreshToken);
                    await _unitOfWork.Save();
                    return;
                }
            }

            await next(context);
        }

        private bool IsTokenExpired(string token)
        {
            try
            {
                var handler = new JwtSecurityTokenHandler();
                var jwtToken = handler.ReadJwtToken(token);

                return jwtToken.ValidTo < DateTime.UtcNow;
            }
            catch
            {
                return true;
            }
        }
    }
}
