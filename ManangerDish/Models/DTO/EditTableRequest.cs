using ManagerDish.Models.Enum;
using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class EditTableRequest
    {
        [Required]
        public int Id { get; set; }

        [Required]
        [Range(1, 10)]
        public int Capicity { get; set; } = 1;

        [Required]
        public TableStatus Status { get; set; } = TableStatus.Available;

        [Required]
        public string Token { get; set; } = Guid.NewGuid().ToString();
    }
}
