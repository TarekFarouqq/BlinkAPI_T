using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Discount
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DiscountId { get; set; }
        [Required]
        [Range(1, 100, ErrorMessage = "Discount percentage must be between 1 and 100.")]
        public int DiscountPercentage { get; set; }
        [Required]
        public DateTime DiscountFromDate { get; set; }
        [Required]
        public DateTime DiscountEndDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new HashSet<ProductDiscount>();
        


    }
}


