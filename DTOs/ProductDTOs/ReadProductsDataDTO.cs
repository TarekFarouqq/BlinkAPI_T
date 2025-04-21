namespace Blink_API.DTOs.ProductDTOs
{
    public class ReadProductsDataDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public List<ReadProductStockDataDTO> StockProductInventories { get; set; }
    }
    public class ReadProductStockDataDTO
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public decimal StockUnitPrice { get; set; }
        public int StockQuantity { get; set; }
    }
}
