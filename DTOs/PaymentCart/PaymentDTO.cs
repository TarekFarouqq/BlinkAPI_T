namespace Blink_API.DTOs.PaymentCart
{
    public class PaymentDTO
    {
        public string PaymentIntentId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Amount { get; set; }
    }
}
