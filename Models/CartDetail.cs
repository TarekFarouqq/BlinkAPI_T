using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class CartDetail
    {
        public int Quantity { get; set; }
        [Required]
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Required]
        public int CartId { get; set; }
        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }

        // fleunt pk
    }
}


