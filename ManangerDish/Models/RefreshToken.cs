using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace ManagerDish.Models
{
    public class RefreshToken
    {
        [Key]
        public string Token { get; set; } = Guid.NewGuid().ToString();

        public int AccountId { get; set; }
        [ForeignKey("AccountId")]
        public virtual Account? Account { get; set; }

        [Required]
        public DateTime Expiration { get; set; }
        public bool IsExpired => DateTime.UtcNow >= Expiration;

        [Required]
        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}
