using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public class CustomerCart
    {
        public string Id { get; set; }
        public List<CartDetail> CartDetails { get; set; }
        public int? DeliveryMethodId { get; set; }
        public decimal ShippingPrice { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public CustomerCart(string id)
        {
            Id = id; 
        }
    }
}
