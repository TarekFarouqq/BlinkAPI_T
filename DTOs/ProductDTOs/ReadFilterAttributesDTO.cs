namespace Blink_API.DTOs.ProductDTOs
{
    public class ReadFilterAttributesDTO
    {
        public int attributeId { get; set; }
        public string attributeName { get; set; }
        public string attributeType { get; set; }
        public bool hasDefaultAttributes { get; set; }
        public ICollection<ReadDefaultAttributesDTO> DefaultAttributes { get; set; }
    }
    public class ReadDefaultAttributesDTO
    {
        public int defaultAttributeId { get; set; }
        public int attributeId { get; set; }
        public string attributeValue { get; set; }
    }
}
