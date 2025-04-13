using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class ReviewSuppliedProductImages
    {
        public int RequestId { get; set; }
        public string ImagePath { get; set; }
        [ForeignKey("RequestId")]
        public ReviewSuppliedProduct ReviewSuppliedProduct { get; set; }
    }
}
