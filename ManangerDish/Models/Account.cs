using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class Account
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; } = "";

        [Required]
        [StringLength(50)]
        [EmailAddress]
        public string Email { get; set; } = "";

        [Required]
        [StringLength(50)]
        [DataType(DataType.Password)]
        public string Password { get; set; } = "";

        [Required]
        [DataType(DataType.Text)]
        public string? Avatar { get; set; }

        [Required]
        public RoleEnum roleId { get; set; }
        [ForeignKey("roleId")]
        public virtual Role? Role { get; set; }

        public int? CreatedBy { get; set; }
        [ForeignKey("CreatedBy")]
        public virtual Account? CreatedByAccount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
        public bool isActive { get; set; } = true;

        public virtual IEnumerable<OrderDetail>? OrderDetails { get; set; } = new List<OrderDetail>();
        public virtual IEnumerable<Payment>? Payments { get; set; } = new List<Payment>();
        public virtual IEnumerable<RefreshToken>? RefreshTokens { get; set; } = new List<RefreshToken>();

    }
}
