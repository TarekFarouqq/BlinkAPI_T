namespace Blink_API.DTOs.PaymentCart
{
    public class ConfirmPaymentDTO
    {
        public string paymentIntentId { get; set; }
        public bool isSucceeded { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }

    }
}
