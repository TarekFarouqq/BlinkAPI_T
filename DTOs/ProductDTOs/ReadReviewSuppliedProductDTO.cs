namespace Blink_API.DTOs.ProductDTOs
{
    public class ReadReviewSuppliedProductDTO
    {
        public int RequestId { get; set; }
        public DateTime RequestDate { get; set; }
        public bool? RequestStatus { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string SupplierId { get; set; }
        public string SupplierName { get; set; }
        public int InventoryId { get; set; }
        public string InventoryName { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
