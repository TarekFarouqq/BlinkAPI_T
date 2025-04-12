namespace Blink_API.DTOs.InventoryDTOS
{
    public class AddInventoryDTO
    {
        public string InventoryName { get; set; }
        public string InventoryAddress { get; set; }
        public string Phone { get; set; }
        public int BranchId { get; set; }
        public decimal Lat { get; set; }
        public decimal Long { get; set; }
    }
}
