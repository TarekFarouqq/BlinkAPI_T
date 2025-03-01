using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Brand
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BrandId { get; set; }
        [Required]
        [StringLength(50)]
        public string BrandName { get; set; }

        public string BrandImage { get; set; }
        [StringLength(500)]
        public string BrandDescription { get; set; }
        [Url]
        public string BrandWebSiteURL { get; set; }
        public bool IsDeleted { get; set; } = false;

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}


