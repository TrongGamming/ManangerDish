using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using ManagerDish.Models;
using ManagerDish.Models.DTO;
using ManagerDish.Repository.IRepository;
using Microsoft.IdentityModel.Tokens;

namespace ManagerDish.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(IConfiguration configuration, IUnitOfWork unitOfWork, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _httpContextAccessor = httpContextAccessor;

            JwtSecurityTokenHandler.DefaultInboundClaimTypeMap.Clear();
            JwtSecurityTokenHandler.DefaultOutboundClaimTypeMap.Clear();
        }

        public TokenResponse GenerateTokenResponse(Account account)
        {
            var accessToken = GenerateAccessToken(account);
            var refreshToken = GenerateRefreshTokenString(accessToken, account.Id);
            return new TokenResponse
            {
                AccessToken = accessToken,
                RefreshToken = refreshToken,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:AccessTokenExpirationMinutes"]))
            };
        }

        public string GenerateAccessToken(Account account)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var accessTokenExpirationMinutes = Convert.ToInt32(jwtSettings["AccessTokenExpirationMinutes"]);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, account.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Name, account.Name)
            };

            if (!string.IsNullOrEmpty(account.Role?.RoleName))
            {
                claims.Add(new Claim(ClaimTypes.Role, account.Role.RoleName));
            }

            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(accessTokenExpirationMinutes);
            var accessTokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiration,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(
                    new SymmetricSecurityKey(secretKey),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var accessToken = tokenHandler.CreateToken(accessTokenDescriptor);
            var accessTokenString = tokenHandler.WriteToken(accessToken);

            return accessTokenString;
        }

        public string GenerateRefreshTokenString(string accessTokenString, int accountId)
        {
            if (accountId <= 0) throw new ArgumentException("Invalid Account ID");

            var randomNumber = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomNumber);

            var jwtSettings = _configuration.GetSection("Jwt");
            var refreshTokenExpirationDays = Convert.ToInt32(jwtSettings["RefreshTokenExpirationDays"]);
            var refreshToken = Convert.ToBase64String(randomNumber);
            var refreshTokenExpiration = DateTime.UtcNow.AddDays(refreshTokenExpirationDays);

            var refreshTokenEntity = new RefreshToken
            {
                Token = refreshToken,
                Expiration = refreshTokenExpiration,
                AccountId = accountId,
                AccessToken = accessTokenString,
            };

            _unitOfWork.RefreshToken.Create(refreshTokenEntity);
            _unitOfWork.Save();

            return refreshToken;
        }

        public async Task<TokenResponse> ValidateAndRefreshTokens(string expiredAccessToken, string refreshToken)
        {
            var storedRefreshToken = await _unitOfWork.RefreshToken.Get(x => x.Token == refreshToken);

            if (storedRefreshToken == null || storedRefreshToken.IsExpired)
            {
                if (storedRefreshToken != null)
                {
                    _unitOfWork.RefreshToken.Remove(storedRefreshToken);
                    await _unitOfWork.Save();
                    _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");

                }
                throw new UnauthorizedAccessException("Invalid or expired refresh token");
            }

            var principal = GetPrincipalFromExpiredToken(expiredAccessToken);
            if (principal == null)
            {
                throw new SecurityTokenException("Invalid access token");
            }

            var subClaim = principal.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;
            if (!int.TryParse(subClaim, out int accountIdFromAccessToken))
            {
                throw new SecurityTokenException("Invalid access token");
            }

            if (accountIdFromAccessToken != storedRefreshToken.AccountId)
            {
                throw new SecurityTokenException("Access token and refresh token do not match");
            }

            var account = await _unitOfWork.Account.Get(x => x.Id == accountIdFromAccessToken);
            if (account == null)
            {
                throw new UnauthorizedAccessException("Account not found");
            }

            var newAccessToken = GenerateAccessToken(account);
            return new TokenResponse
            {
                AccessToken = newAccessToken,
                RefreshToken = storedRefreshToken.Token,
                AccessTokenExpiration = DateTime.UtcNow.AddMinutes(Convert.ToInt32(_configuration["Jwt:AccessTokenExpirationMinutes"]))
            };
        }

        private ClaimsPrincipal GetPrincipalFromExpiredToken(string token)
        {
            var jwtSettings = _configuration.GetSection("Jwt");
            var secretKey = Encoding.ASCII.GetBytes(jwtSettings["SecretKey"]);
            var tokenHandler = new JwtSecurityTokenHandler();

            var validationParameters = new TokenValidationParameters
            {
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(secretKey),
                ValidateIssuer = true,
                ValidIssuer = jwtSettings["Issuer"],
                ValidateAudience = true,
                ValidAudience = jwtSettings["Audience"],
                ValidateLifetime = false, // Cho phép token hết hạn
                
            };

            try
            {
                var principal = tokenHandler.ValidateToken(token, validationParameters, out var validatedToken);
                var jwtToken = validatedToken as JwtSecurityToken;

                if (jwtToken == null || !jwtToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase))
                {
                    throw new SecurityTokenException("Invalid token algorithm");
                }

                return principal;
            }
            catch (Exception ex)
            {
                throw new SecurityTokenException("Invalid token", ex);
            }
        }

        public async Task<Account> AuthenticateAccount(string username, string password)
        {
            var account = await _unitOfWork.Account.Get(a => a.Email == username);
            if (account != null && BCrypt.Net.BCrypt.Verify(password, account.Password))
            {
                return account;
            }
            return null;
        }
    }
}
