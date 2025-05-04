using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ManagerDish.Models
{
    public class Category
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

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public virtual IEnumerable<Dish>? Dishes { get; set; } = new List<Dish>();

    }
}
