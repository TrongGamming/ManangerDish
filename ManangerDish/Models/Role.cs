
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ManagerDish.Models.Enum;

namespace ManagerDish.Models
{
    public class Role
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public RoleEnum Id { get; set; }


        [Required]
        [StringLength(50)]
        public string RoleName { get; set; } = "";

        [Required]
        [StringLength(50)]
        public string RoleDescription { get; set; } = "";


        public IEnumerable<Account>? Accounts { get; set; } = new List<Account>();

    }
}
