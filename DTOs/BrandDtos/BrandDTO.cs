using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Blink_API.DTOs.Product;
using Blink_API.Models;

namespace Blink_API.DTOs.BrandDtos
{
    public class BrandDTO
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public string BrandDescription { get; set; }
        public string BrandWebSiteURL { get; set; }

         
      // public List<ProductDetailsDTO> Products { get; set; }
    }
}
