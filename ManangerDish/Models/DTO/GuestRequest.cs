using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class GuestRequest
    {
        [Required]
        public string Name { get; set; } = "";

        [Required]
        [DataType(DataType.PhoneNumber)]
        [StringLength(10, ErrorMessage = "Phone number must be 10 digits.")]
        public string PhoneNumber { get; set; } = "";
    }
}
