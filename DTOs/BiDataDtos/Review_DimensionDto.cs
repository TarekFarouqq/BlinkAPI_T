namespace Blink_API.DTOs.BiDataDtos
{
    public class Review_DimensionDto
    {
        public int ReviewId { get; set; }
        public int Rate { get; set; }
        public DateTime CreationDate { get; set; }
        public string UserId { get; set; }

        public string ProductId { get; set; }
        public List<string> ReviewComments { get; set; }

    }
}
