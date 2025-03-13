using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Category
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CategoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string CategoryName { get; set; }
        [StringLength(500)]
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }

        public bool IsDeleted { get; set; } = false;
        public int? ParentCategoryId { get; set; }

        [ForeignKey("ParentCategoryId")]
        public virtual Category ParentCategory { get; set; } // Reference to Parent

        public virtual ICollection<Category> SubCategories { get; set; } = new HashSet<Category>(); // Children

        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();

    }
}


