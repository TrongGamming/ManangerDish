namespace ManagerDish.Models.DTO
{
    public class TokenResponse
    {
        public string AccessToken { get; set; } = "";
        public string RefreshToken { get; set; } = "";
        public DateTime AccessTokenExpiration { get; set; }
    }
}
