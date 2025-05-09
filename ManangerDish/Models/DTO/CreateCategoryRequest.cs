using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class CreateCategoryRequest
    {
        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string Description { get; set; } = "";
    }
}
