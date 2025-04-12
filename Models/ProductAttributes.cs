using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class ProductAttributes
    {
        public int ProductId { get; set; }
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
        [ForeignKey("ProductId")]
        public Product Product { get; set; }
        [ForeignKey("AttributeId")]
        public FilterAttributes FilterAttribute { get; set; }
    }
    
}
