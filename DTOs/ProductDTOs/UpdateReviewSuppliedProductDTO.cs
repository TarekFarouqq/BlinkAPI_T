namespace Blink_API.DTOs.ProductDTOs
{
    public class UpdateReviewSuppliedProductDTO
    {
        public bool? RequestStatus { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public string SupplierId { get; set; }
        public int InventoryId { get; set; }
        public double ProductPrice { get; set; }
        public int ProductQuantity { get; set; }
    }
}
