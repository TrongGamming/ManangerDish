using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    [Table("Tables")]
    public class Table
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Number { get; set; }

        [Required]
        [Range(1,10)]
        public int Capicity { get; set; } = 1;

        [Required]
        public TableStatus Status { get; set; } = TableStatus.Available;

        [Required]
        public string Token { get; set; } = Guid.NewGuid().ToString();

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now; 

        public virtual IEnumerable<Guest>? Guests { get; set; } = new List<Guest>();
        public virtual IEnumerable<Order>? Orders { get; set; } = new List<Order>();

    }
}
