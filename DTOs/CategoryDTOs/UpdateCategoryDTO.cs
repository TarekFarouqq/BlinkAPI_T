namespace Blink_API.DTOs.CategoryDTOs
{
    public class UpdateCategoryDTO
    {
        public string CategoryName { get; set; }
        public string CategoryDescription { get; set; }
        public string CategoryImage { get; set; }
        public int? ParentCategoryId { get; set; }
    }
}
