namespace Blink_API.DTOs.WishListDTOs
{
    public class ReadWishListDTO
    {
        public string UserId { get; set; }
        public int WishListId { get; set; }

        public ICollection<WishListDetailsDTO> WishListDetails { get; set; } = new List<WishListDetailsDTO>();
    }
    public class WishListDetailsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductImageUrl { get; set; }

    }
}

