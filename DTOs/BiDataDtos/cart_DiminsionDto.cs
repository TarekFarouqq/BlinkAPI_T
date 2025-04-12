namespace Blink_API.DTOs.BiDataDtos
{
    public class cart_DiminsionDto
    {
        public int CartId { get; set; }
        public string UserId { get; set; }
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
    }
}
