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
        public ICollection<InsertProductImagesDTO> ProductImages{ get; set; }
    }
    public class InsertProductImagesDTO
    {
        // Manuall Increase
        //public int ProductImageId { get; set; }
        public int ProductId { get; set; }
        public IFormFile ProductImage { get; set; }
        //public string ImagePath { get; set; }
    }
}
