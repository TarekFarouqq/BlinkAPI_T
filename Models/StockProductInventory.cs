using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class StockProductInventory
    {
        [Required]
        public int InventoryId { get; set; }
        [Required]

        public int ProductId { get; set; }
        [ForeignKey("InventoryId")]
        public virtual Inventory Inventory { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        public decimal StockUnitPrice { get; set; }
        public int StockQuantity { get; set; }
        public bool IsDeleted { get; set; } = false;
        // Fleunt pk
    }
}


