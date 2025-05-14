using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class CreateAccountRequest
    {
        [Required]
        [StringLength(50, MinimumLength = 3, ErrorMessage = "Tên tài khoản phải từ 3 đến 50 ký tự")]
        public string UserName { get; set; } = string.Empty;

        [Required]
        [StringLength(50, MinimumLength = 6, ErrorMessage = "Mật khẩu phải từ 6 đến 50 ký tự")]
        public string Password { get; set; } = string.Empty;

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Mật khẩu không khớp")]
        public string ConfirmPassword { get; set; } = string.Empty;

        [Required]
        public int RoleId { get; set; }

        [Required]
        [EmailAddress(ErrorMessage = "Email không hợp lệ")]
        public string Email { get; set; } = string.Empty;


        [DataType(DataType.Text)]
        public IFormFile? Avatar { get; set; } = null!;

        [Required]
        [DataType(DataType.Text)]
        public string? ImagePath { get; set; }
    }
}
