using Blink_API.DTOs.Category;
using Blink_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly CategoryService categoryService;
        public CategoryController(CategoryService _categoryService)
        {
            categoryService = _categoryService;
        }
        [HttpGet("GetParentCategories")]
        public async Task<ActionResult> GetParentCategories()
        {
            var categories = await categoryService.GetParentCategories();
            if (categories == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var category in categories)
            {
                category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            }
            return Ok(categories);
        }
        [HttpGet("GetChildCategories")]
        public async Task<ActionResult> GetChildCategories()
        {
            var categories = await categoryService.GetChildCategories();
            if (categories == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var category in categories)
            {
                category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            }
            return Ok(categories);
        }
      
    }
}
