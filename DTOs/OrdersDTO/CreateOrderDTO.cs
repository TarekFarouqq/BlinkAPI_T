namespace Blink_API.DTOs.OrdersDTO
{
    public class CreateOrderDTO
    {

        public string UserId { get; set; }

        public string Address { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }

        public string PhoneNumber { get; set; }

        public string PaymentMethod { get; set; }  
    }
}
