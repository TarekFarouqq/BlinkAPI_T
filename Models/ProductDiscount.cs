using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class ProductDiscount
    {
        [Required]
        public int DiscountId { get; set; }
        [Required]

        public int ProductId { get; set; }
        [ForeignKey("DiscountId")]
        public virtual Discount Discount { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDeleted { get; set; } = false;
    }
}


