namespace Blink_API.DTOs.OrdersDTO
{
    public class orderDTO
    {       
            public int OrderId { get; set; }
            public string OrderStatus { get; set; }
            public DateTime OrderDate { get; set; }

            public decimal Subtotal { get; set; }
            public decimal Tax { get; set; }
            public decimal Shipping { get; set; }
            public decimal Total { get; set; }

            public string PaymentIntentId { get; set; }

            public List<ConfirmedOrderItemDTO> Items { get; set; } = new();
        
    }

    public class ConfirmedOrderItemDTO
    {
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

}
