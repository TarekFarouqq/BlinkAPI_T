using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class TransactionProduct
    {
        [Required]
        public int TransactionQuantity { get; set; }
        [Required]

        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [Required]

        public int InventoryTransactionId { get; set; }
        [ForeignKey("InventoryTransactionId")]
        public virtual  InventoryTransactionHeader InventoryTransactionHeader { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}


