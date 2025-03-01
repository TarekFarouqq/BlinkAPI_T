using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class TransactionDetail
    {
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        [Required]
        public int InventoryTransactionHeaderId { get; set; }
        [ForeignKey("InventoryTransactionHeaderId")]
        public virtual InventoryTransactionHeader InventoryTransactionHeader { get; set; }
        [Required]
        public int SrcInventoryId { get; set; }
        [Required]
        public int DistInventoryId { get; set; }

        [ForeignKey("SrcInventoryId")]
        public virtual Inventory SrcInventory { get; set; }

        [ForeignKey("DistInventoryId")]
        public virtual Inventory DistInventory { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}
