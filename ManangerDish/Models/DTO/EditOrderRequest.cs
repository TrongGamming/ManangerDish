using System.ComponentModel.DataAnnotations;

namespace ManagerDish.Models.DTO
{
    public class EditOrderRequest
    {
        [Required(ErrorMessage = "Order ID is required")]
        public int OrderId { get; set; }

        public List<GuestOrderItemDTO> GuestOrder { get; set; } = new List<GuestOrderItemDTO>();

        public bool HasValidDishes()
        {
            return GuestOrder.Any(od => od.Quality > 0);
        }
    }
}