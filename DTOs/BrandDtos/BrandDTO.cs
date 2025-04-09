using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.BrandDtos
{
    public class BrandDTO
    {
        public int BrandId { get; set; }
        public string BrandName { get; set; }
        public string BrandImage { get; set; }
        public string BrandDescription { get; set; }
        public string BrandWebSiteURL { get; set; }

        // product within this brand :
       // public List<string> Products { get; set; }


    }
}
