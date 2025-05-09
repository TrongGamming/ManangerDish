using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class CreateOrderRequest
    {
        [Required]
        public int TableId { get; set; }

        public int GuestId { get; set; }

        public String NewGuestName { get; set; } = "";
        [Phone]
        public String NewGuestPhone { get; set; } = "";

        public List<GuestOrderItemDTO> GuestOrder { get; set; } = new();

    }
}
