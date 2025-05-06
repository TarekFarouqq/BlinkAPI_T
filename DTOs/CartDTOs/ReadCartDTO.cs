using Blink_API.DTOs.Product;

namespace Blink_API.DTOs.CartDTOs
{
    public class ReadCartDTO
    {
        public string UserId { get; set; }
        public int CartId { get; set; }

        public ICollection<CartDetailsDTO> CartDetails { get; set; } = new List<CartDetailsDTO>();
    }
    public class CartDetailsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public decimal ProductUnitPrice { get; set; }
        public string ProductImageUrl { get; set; }
        public decimal DiscountAmount { get; set; }
        public int Quantity { get; set; }
    }
}
