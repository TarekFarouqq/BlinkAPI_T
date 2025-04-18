using Blink_API.Models;

namespace Blink_API.DTOs.TransferProductsDTOs
{
    public class InsertInputTransferProductDTO
    {
        public DateTime InventoryTransactionDate { get; set; }= DateTime.UtcNow;
        public int InventoryTransactionType { get; set; } = 1;
        public ICollection<InsertInputTrasactionProductDTO> TransactionProducts { get; set; }
    }
    public class InsertInputTrasactionProductDTO
    {
        public int TransactionQuantity { get; set; }
        public int ProductId { get; set; }
    }
    public class ReadInventoryTransactions
    {
        public int InventoryTransactionHeaderId { get; set; }
        public DateTime InventoryTransactionDate { get; set; }
        public int SrcInventoryId { get; set; }
        public string SrcInventoryName { get; set; }
        public int DistInventoryId { get; set; }
        public string DistInventoryName { get; set; }
        public List<ReadTrasactionProductsDTO> TransactionProducts { get; set; }
    }
    public class ReadTrasactionProductsDTO
    {
        public int TransactionQuantity { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
    }
    public class InsertTransactionHistoryDTO
    {
        public int InventoryTransactionType { get; set; }
        public InsertTransactionDetailsDTO TransactionDetail { get; set; }
        public ICollection<InsertInputTrasactionProductDTO> TransactionProducts { get; set; }
    }
    public class InsertTransactionDetailsDTO
    {
        public string UserId { get; set; }
        public int SrcInventoryId { get; set; }
        public int DistInventoryId { get; set; }
    }
}
