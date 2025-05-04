using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerDish.Models
{
    public class DishSnapshot
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        public int DishId { get; set; }
        [ForeignKey("DishId")]
        public virtual Dish? Dish { get; set; }

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

        public DateTime CreatedAt { get; set; } = DateTime.Now;


    }
}
