using Blink_API.Models;

namespace Blink_API.DTOs.BiDataDtos
{
    public class InventoryTransaction_FactDto
    {

        public int SrcInventoryId { get; set; }
        public int DistInventoryId { get; set; }
        public string UserId { get; set; }
        public int InventoryTransactionHeaderId { get; set; }

        public int ProductId { get; set; }

        public int Quantity { get; set; }


        public DateTime TransactionDate { get; set; }

        public string TransactionType { get; set; }  

        public string UserName { get; set; }
        //public virtual ICollection<TransactionProduct> TransactionProducts { get; set; } = new HashSet<TransactionProduct>();

    }
}
