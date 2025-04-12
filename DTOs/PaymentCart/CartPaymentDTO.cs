using Blink_API.Models;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.PaymentCart
{
    public class CartPaymentDTO // Customer Cart 
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
        public string PaymentMethod { get; set; }  
        public string PaymentStatus { get; set; }  
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }

    }
  
}

