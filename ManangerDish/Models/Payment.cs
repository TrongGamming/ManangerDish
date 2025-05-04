using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int Amount { get; set; }

        [Required]
        public int orderId { get; set; }
        [ForeignKey("orderId")]
        public virtual Order? Order { get; set; }

        [Required]
        public PaymentMethod PaymentMethod { get; set; } = PaymentMethod.Cash;

        [Required]
        public int ProcessedBy { get; set; }
        [ForeignKey("ProcessedBy")]
        public virtual Account? Account { get; set; }


    }
}
