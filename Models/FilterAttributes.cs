using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Models
{
    public class FilterAttributes
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AttributeId { get; set; }
        [Required]
        public string AttributeName { get; set; }
        [Required]
        public string AttributeType { get; set; }
        [Required]
        public bool HasDefaultAttributes { get; set; } = false;
        public virtual ICollection<DefaultAttributes> DefaultAttributes { get; set; }
    }
    
}
