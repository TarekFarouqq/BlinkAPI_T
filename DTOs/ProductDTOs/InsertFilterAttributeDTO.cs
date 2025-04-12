namespace Blink_API.DTOs.ProductDTOs
{
    public class InsertFilterAttributeDTO
    {
        public string AttributeName { get; set; }
        public string AttributeType { get; set; }
        public bool HasDefaultAttributes { get; set; } = false;
    }
}
