using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.CategoryDTOs
{
    public class CreateCategoryDTO
    {
        [Required]
        [MaxLength(50)]
        public string CategoryName { get; set; }

        [MaxLength(500)]
        public string CategoryDescription { get; set; }

        public string CategoryImage { get; set; }

        public int? ParentCategoryId { get; set; } // check lw parent is equal null wla l2a 
    }
}
