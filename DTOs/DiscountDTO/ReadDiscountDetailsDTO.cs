using Blink_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.DiscountDTO
{
    public class ReadDiscountDetailsDTO
    {
        public int DiscountId { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime DiscountFromDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public ICollection<ReadProductsDiscountDTO> ReadProductsDiscountDTOs{ get; set; }
    }
    public class ReadProductsDiscountDTO
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SubCategoryName { get; set; }
        public string ParentCategoryName { get; set; }
        public string BrandName { get; set; }
        public decimal ProductPrice { get; set; }
        public decimal DiscountAmount { get; set; }
    }
    public class InsertDiscountDetailsDTO
    {
        public int DiscountPercentage { get; set; }
        public DateTime DiscountFromDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public List<InsertProductDiscountDetailsDTO> InsertProductDiscountDetails { get; set; }
    }
    public class InsertProductDiscountDetailsDTO
    {
        public int ProductId { get; set; }
        public decimal DiscountAmount { get; set; }
    }
    public class UpdateDiscountDetailsDTO
    {
        public int DiscountId { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime DiscountFromDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public List<InsertProductDiscountDetailsDTO> UpdateProductDiscountDetails { get; set; }
    }
    public class UpdateProductDiscountDetailsDTO
    {
        public int ProductId { get; set; }
        public decimal DiscountAmount { get; set; }
    }
}
