using ManagerDish.Models.Enum;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class CreateDishRequest
    {
        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [StringLength(50)]
        public string Description { get; set; } = "";

        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        [DataType(DataType.Currency, ErrorMessage = "Giá tiền nhập không hợp lệ")]
        public int Price { get; set; } = 0;

        [DataType(DataType.Text)]
        public IFormFile? Image { get; set; }

        [DataType(DataType.Text)]
        public string? ImagePath { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Required(ErrorMessage = "Vui lòng nhập trường này")]
        public DishStatus Status { get; set; } = DishStatus.Available;
    }
}
