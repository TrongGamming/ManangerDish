using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class CreateTableRequest
    {
        [Required]
        [Range(1, 10)]
        public int Capicity { get; set; } = 1;

        [Required]
        public TableStatus Status { get; set; } = TableStatus.Available;

        [Required]
        public string Token { get; set; } = Guid.NewGuid().ToString();

    }
}
