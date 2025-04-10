using Blink_API.DTOs.InventoryDTOS;

namespace Blink_API.DTOs.BranchDto
{
    public class ReadBranchDTO
    {
        public int BranchId { get; set; }
        public string BranchName { get; set; }
        public string BranchAddress { get; set; }
        public string Phone { get; set; }
        public List<ReadInventoryDTO> Inventories { get; set; }
    }
}
