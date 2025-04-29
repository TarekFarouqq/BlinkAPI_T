using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blink_API.Models;

namespace Blink_API.DTOs.ProductDTOs
{
    public class InsertProductDTO
    {
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SupplierId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> ProductImages { get; set; } = new List<IFormFile>();
        public List<string> OldImages { get; set; } = new List<string>();
        public List<InsertProductStockDTO> ProductStocks { get; set; }
    }
    public class InsertProductStockDTO
    {
        public int InventoryId { get; set; }
        public int ProductId { get; set; }
        public decimal StockUnitPrice { get; set; }
        public int StockQuantity { get; set; }
    }
    public class UpdateProductDTO
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductDescription { get; set; }
        public string SupplierId { get; set; }
        public int BrandId { get; set; }
        public int CategoryId { get; set; }
        public List<IFormFile> NewProductImages { get; set; } = new List<IFormFile>();
        public List<string> OldProductImages { get; set; } = new List<string>();
        public List<InsertProductStockDTO> ProductStocks { get; set; }
    }
    public class InsertProductImagesDTO
    {
        public int ProductId { get; set; } 
        public string? ProductImagePath { get; set; }
    }
}
