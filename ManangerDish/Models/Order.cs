using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerDish.Models
{
    public class Order
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int TableId { get; set; }
        [ForeignKey("TableId")]
        public virtual Table? Table { get; set; }

        public bool Paid { get; set; } = false;
        public int Total { get; set; } = 0;

        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; } 
        public virtual IEnumerable<Guest>? Guests { get; set; } = new List<Guest>();

    }
}
