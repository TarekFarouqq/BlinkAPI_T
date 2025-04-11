using Blink_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.ProductDTOs
{
    public class InsertFilterAttribute
    {
        public string AttributeName { get; set; }
        public string AttributeType { get; set; }
        public bool HasDefaultAttributes { get; set; } = false;
    }
    public class InsertDefaultAttributes
    {
        public int AttributeId { get; set; }
        public string AttributeValue { get; set; }
    }
}
