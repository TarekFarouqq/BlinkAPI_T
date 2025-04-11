using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class DefaultAttributes
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DefaultAttributeId { get; set; }
        public int AttributeId { get; set; }
        [Required]
        public string AttributeValue { get; set; }
        [ForeignKey("AttributeId")]
        public FilterAttributes FilterAttribute { get; set; }
    }
    
}
