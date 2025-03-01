using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Inventory
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int InventoryId { get; set; }
        [Required]
        [StringLength(50)]
        public string InventoryName { get; set; }
        [Required]
        [StringLength(200)]
        public string InventoryAddress { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        [Required]

        public int BranchId { get; set; }

        [ForeignKey("BranchId")]
        public virtual Branch Branch { get; set; }
        public bool IsDeleted { get; set; } = false;


        [InverseProperty("SrcInventory")]
        public virtual ICollection<TransactionDetail> SentTransactions { get; set; } = new HashSet<TransactionDetail>();

        [InverseProperty("DistInventory")]
        public virtual ICollection<TransactionDetail> ReceivedTransactions { get; set; } = new HashSet<TransactionDetail>();
        public virtual ICollection<InventoryTransactionHeader> InventoryTransactionHeaders { get; set; } = new HashSet<InventoryTransactionHeader>();
        public virtual ICollection<StockProductInventory> StockProductInventories { get; set; } = new HashSet<StockProductInventory>();

    }
}


