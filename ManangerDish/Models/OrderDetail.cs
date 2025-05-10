using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        public int? OrderId { get; set; }
        [ForeignKey("OrderId")]
        public virtual Order? Order { get; set; }

        [Required]
        public int DishId { get; set; }
        [ForeignKey("DishId")]
        public virtual Dish? Dish { get; set; }

        [Required]
        public int Quantity { get; set; } = 1;

        public int? HandlerId { get; set; } = null;
        [ForeignKey("HandlerId")]
        public virtual Account? Handler { get; set; }

        [Required]
        public OrderStatus Status { get; set; } = OrderStatus.Pending;

        [Required]
        public int guestId { get; set; }
        [ForeignKey("guestId")]
        public virtual Guest? Guest { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
