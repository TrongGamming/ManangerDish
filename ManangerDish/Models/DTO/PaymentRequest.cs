using ManagerDish.Models.Enum;

namespace ManagerDish.Models.DTO
{
    public class PaymentRequest
    {
        public Order Order { get; set; } = new Order();
        public decimal Subtotal { get; set; }
        public decimal Tax { get; set; }
        public decimal ServiceCharge { get; set; }
        public decimal Discount { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod SelectedPaymentMethod { get; set; } = PaymentMethod.Cash;
        public decimal ReceivedAmount { get; set; }
        public decimal ChangeAmount { get; set; }
        public string? DigitalProvider { get; set; }
        public List<Payment> PaymentHistory { get; set; } = new List<Payment>();
    }

    public class ProcessPaymentRequest
    {
        public int OrderId { get; set; }
        public decimal Amount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
        public decimal ReceivedAmount { get; set; }
        public string? DigitalProvider { get; set; }
    }

    public class PaymentResponse
    {
        public bool Success { get; set; }
        public string Message { get; set; } = "";
        public int OrderId { get; set; }
        public int PaymentId { get; set; }
        public decimal ChangeAmount { get; set; }
    }
}
