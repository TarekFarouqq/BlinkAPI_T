using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class ProductImage
    {
        // id is not identity !!!!!!!
        public int ProductImageId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Image path is too long.")]
        public string ProductImagePath { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}


