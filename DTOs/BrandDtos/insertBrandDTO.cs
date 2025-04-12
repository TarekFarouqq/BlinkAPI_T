namespace Blink_API.DTOs.BrandDtos
{
    public class insertBrandDTO
    {
        public string BrandName { get; set; }
        public IFormFile BrandImageFile { get; set; }
        public string BrandDescription { get; set; }
        public string BrandWebSiteURL { get; set; }
    }
}
