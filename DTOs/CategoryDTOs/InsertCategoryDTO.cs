namespace Blink_API.DTOs.CategoryDTOs
{
    public class InsertCategoryDTO
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public IFormFile CategoryImage { get; set; }
        public List<InsertChildCategoryDTO> SubCategories { get; set; }
    }
    public class InsertChildCategoryDTO
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public IFormFile CategoryImage { get; set; }
    }
    public class UpdateParentCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string? OldImage { get; set; }
        public IFormFile? NewImage { get; set; }
        public List<UpdateChildCategoryDTO> SubCategories { get; set; }
    }
    public class UpdateChildCategoryDTO
    {
        public int CategoryId { get; set; }
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string? OldImage { get; set; }
        public IFormFile? NewImage { get; set; }
    }
    public class UpdateParentCategoryDTOo
    {

    }
}
