using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.ProductDTOs
{
    public class InsertProductDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SupplierId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> ProductImages { get; set; }
    }
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SupplierId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> NewProductImages { get; set; }
        public List<string> OldProductImages { get; set; }
    }
    public class InsertProductImagesDTO
    {
        public int ProductId { get; set; } 
        public string? ProductImagePath { get; set; }
    }
}
