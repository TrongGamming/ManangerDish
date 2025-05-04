using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class Dish
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string Description { get; set; } = "";

        [Required]
        [DataType(DataType.Currency)]
        public int Price { get; set; } = 0;

        [Required]
        [DataType(DataType.Text)]
        public string? Image { get; set; }

        [Required]
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category? Category { get; set; }

        [Required]
        public DishStatus Status { get; set; } = DishStatus.Available;

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        public virtual IEnumerable<DishSnapshot>? DishSnapshots { get; set; } = new List<DishSnapshot>();
        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
    }
}
