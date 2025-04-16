using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.BiDataDtos
{
    public class Product_DiminsionDto
    {
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public string SupplierId { get; set; }
        public int ParentCategoryId { get; set; }
        public string ProductDescription { get; set; }

        public DateTime ProductCreationDate { get; set; } = DateTime.UtcNow;
        public DateTime ProductModificationDate { get; set; } = DateTime.UtcNow;
        public DateTime ProductSupplyDate { get; set; } = DateTime.UtcNow;

        public string CategoryName { get; set; }
       
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }

        public string BrandName { get; set; }
        public int BrandId { get; set; }
        public string BrandImage { get; set; }
   
        public string BrandDescription { get; set; }


        public List<string> ProductImagePaths { get; set; }

        public string BrandWebSiteURL { get; set; }



    }
}
