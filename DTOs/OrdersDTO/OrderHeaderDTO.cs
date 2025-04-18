using Blink_API.DTOs.PaymentCart;

namespace Blink_API.DTOs.OrdersDTO
{
    public class OrderHeaderDTO
    {
        public int OrderHeaderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderSubtotal { get; set; }
        public decimal OrderTax { get; set; }
        public decimal OrderShippingCost { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; }

        public int PaymentId { get; set; }
        public PaymentDTO Payment { get; set; }

        public List<orderDTO> OrderDetails { get; set; } = new();

        public bool IsPaymentReceived { get; set; }
    }
}
