namespace Blink_API.DTOs.BiDataDtos
{
    public class Payment_DimensionDto
    {
        public int PaymentId { get; set; }
        public string Method { get; set; }
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
    }
}
