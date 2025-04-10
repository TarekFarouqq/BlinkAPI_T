using Blink_API.DTOs.Product;

namespace Blink_API.DTOs.ProductDTOs
{
    public class ProductDiscountsDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public DateTime ProductCreationDate { get; set; }
        public DateTime ProductModificationDate { get; set; }
        public DateTime ProductSupplyDate { get; set; }
        public List<string> ProductImages { get; set; }
        public string SupplierId  { get; set; }
        public string SupplierName { get; set; }
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public double AverageRate { get; set; }
        public int CountOfRates { get; set; }
        public bool IsDeleted { get; set; }
        public decimal ProductPrice { get; set; }
        public int StockQuantity { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public ICollection<ReviewCommentDTO> ProductReviews { get; set; } = new List<ReviewCommentDTO>();
    }
}
