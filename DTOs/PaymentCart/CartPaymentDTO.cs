using Blink_API.DTOs.CartDTOs;

namespace Blink_API.DTOs.PaymentCart
{
    public class CartPaymentDTO // Customer Cart 
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        //public string PaymentMethod { get; set; }
        //public string PaymentStatus { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal TotalAmount { get; set; }

        public List<CartDetailsDTO> Items { get; set; }
    }




}
