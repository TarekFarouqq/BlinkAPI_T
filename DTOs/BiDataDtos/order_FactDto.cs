namespace Blink_API.DTOs.BiDataDtos
{
    public class order_FactDto
    {
        public int OrderId { get; set; }   //
        public int PaymentId { get; set; }//
        public int CartId { get; set; }//
        public int ProductId { get; set; }//
        public int OrderDetailId { get; set; }//
        public DateTime OrderDate { get; set; }//
        public decimal Subtotal { get; set; }//
        public decimal Tax { get; set; }//
        public decimal ShippingCost { get; set; }//
        public decimal TotalAmount { get; set; }//
        public string OrderStatus { get; set; }//
        public int Quantity { get; set; }
        public decimal SellPrice { get; set; }//
    }
}
