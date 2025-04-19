using Blink_API.DTOs.PaymentCart;

namespace Blink_API.DTOs.OrdersDTO
{
    public class OrderToReturnDto
    {
        public int OrderHeaderId { get; set; }
        public DateTime OrderDate { get; set; }
        public decimal OrderTotalAmount { get; set; }
        public string OrderStatus { get; set; }
        public decimal OrderShippingCost { get; set; }
        public decimal OrderTax { get; set; }
        public List<OrderDetailDto> OrderDetails { get; set; }
        public PaymentDto Payment { get; set; }
    }
    public class OrderDetailDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }

        public int SellQuantity { get; set; }
        public decimal SellPrice { get; set; }
    }
    public class PaymentDto
    {
        public string Method { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string  PaymentIntentId { get; set; }
    }
}
