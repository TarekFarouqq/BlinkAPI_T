using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Product
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ProductId { get; set; }
        [Required]
        [StringLength(100)]
        public string ProductName { get; set; }
        [StringLength(500)]
        public string ProductDescription { get; set; }
        [Required]
        public DateTime ProductCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ProductModificationDate { get; set; } = DateTime.UtcNow;
        public DateTime ProductSupplyDate { get; set; } = DateTime.UtcNow;
        public bool IsDeleted { get; set; } = false;
        [Required]
        public string SupplierId { get; set; }
        [Required]
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        [ForeignKey("CategoryId")]
        public virtual Category Category { get; set; }
        [ForeignKey("BrandId")]
        public virtual Brand Brand { get; set; }
        [ForeignKey("SupplierId")]
        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<ProductImage> ProductImages { get; set; } = new HashSet<ProductImage>();
        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; } = new HashSet<TransactionProduct>();
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public virtual ICollection<CartDetail> CartDetails { get; set; } = new HashSet<CartDetail>();
        public virtual ICollection<WishListDetail> WishListDetails { get; set; } = new HashSet<WishListDetail>();
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; } = new HashSet<ProductDiscount>();
        public virtual ICollection<StockProductInventory> StockProductInventories { get; set; } = new HashSet<StockProductInventory>();
        public virtual ICollection<ProductAttributes> ProductAttributes { get; set; }= new HashSet<ProductAttributes>();
    }
}


