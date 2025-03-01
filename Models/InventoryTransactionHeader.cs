using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class InventoryTransactionHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryTransactionHeaderId { get; set; }
        [Required]
        public DateTime InventoryTransactionDate { get; set; }
        [Required]
        [Range(1, 4)]
        public int InventoryTransactionType { get; set; }
        public virtual TransactionDetail TransactionDetail { get; set; } 
        public virtual ICollection<TransactionProduct> TransactionProducts { get; set; } = new HashSet<TransactionProduct>();
        public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();
        public bool IsDeleted { get; set; } = false;


    }
}


