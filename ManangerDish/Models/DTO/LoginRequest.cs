using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class LoginRequest 
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [DataType(DataType.EmailAddress, ErrorMessage = "Trường này phải là Email")]
        public string UserName { get; set; } = string.Empty;
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        public string Password { get; set; } = string.Empty;
    }
}
