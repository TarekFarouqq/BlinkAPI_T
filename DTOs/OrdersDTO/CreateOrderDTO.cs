namespace Blink_API.DTOs.OrdersDTO
{
    public class CreateOrderDTO
    {
        public int CartId { get; set; }
        public string PaymentIntentId { get; set; } = string.Empty;
        public decimal ShippingPrice { get; set; }
        public decimal Tax { get; set; }
    }
}
