namespace Blink_API.DTOs.WishListDTOs
{
    public class ReadWishListDTO
    {
        public string UserId { get; set; }
        public int WishListId { get; set; }

        public ICollection<WishLishDetailsDTO> WishLishDetails { get; set; } = new List<WishLishDetailsDTO>();
    }
    public class WishLishDetailsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }

    }
}

