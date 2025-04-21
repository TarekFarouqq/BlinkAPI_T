namespace Blink_API.DTOs.InventoryDTOS
{
    public class ReadInventoryDTO
    {
        public int InventoryId { get; set; }
        public string InventoryName { get; set; }
        public string InventoryAddress { get; set; }
        public string Phone { get; set; }
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
    }
}
