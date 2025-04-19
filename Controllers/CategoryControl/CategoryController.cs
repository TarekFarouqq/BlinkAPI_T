using Blink_API.DTOs.Category;
using Blink_API.DTOs.CategoryDTOs;
using Blink_API.Repositories.BiDataRepos;
using Blink_API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        [HttpGet("GetParentCategoryById")]
        public async Task<ActionResult> GetParentCategoryById(int id)
        {
            var category = await categoryService.GetParentCategoryById(id);
            if (category == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            return Ok(category);
        }
        [HttpGet("GetChildCategoryById")]
        public async Task<ActionResult> GetChildCategoryById(int id)
        {
            var category = await categoryService.GetChildCategoryById(id);
            if (category == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            return Ok(category);
        }
        [HttpPost("AddCategory")]
        public async Task<ActionResult> AddCategory(CreateCategoryDTO dto)
        {
            if (!ModelState.IsValid)

                return BadRequest();


            var result = await categoryService.AddedCategory(dto);

            if (result.Contains("not exist")) return NotFound(result);
            return Ok(result);



        }
        [HttpDelete("SoftDeleteCategory/{id}")]
        public async Task<ActionResult> SoftDeleteCategory(int id)
        {
            var result = await categoryService.SoftDeleteCategory(id);
            if (result.Contains("not found")) return NotFound(result);
            return Ok(result);
        }
        [HttpPut("UpdateCategory/{id}")]
        public async Task<ActionResult> UpdateCategory(int id, [FromBody] UpdateCategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await categoryService.UpdateCategory(id, dto);
            if (result.Contains("not found") || result.Contains("is deleted"))
            {
                return NotFound(result);
            }

            return Ok(result);
        }
        [HttpGet("GetChildCategoryByParentId")]
        public async Task<ActionResult> GetChildCategoryByParentId(int id)
        {
            var categories = await categoryService.GetChildCategoryByParentId(id);
            if (categories == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var category in categories)
            {
                category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            }
            return Ok(categories);
        }
        #region Sprint3
        [HttpGet("{pgNumber}/{pgSize}")]
        public async Task<ActionResult> GetAll(int pgNumber,int pgSize)
        {
            var categories = await categoryService.GetAll(pgNumber,pgSize);
            if (categories == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            foreach (var category in categories)
            {
                category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            }
            return Ok(categories);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var category = await categoryService.GetById(id);
            if (category == null) return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}";
            category.CategoryImage = baseUrl + category.CategoryImage.Replace("wwwroot", "");
            foreach (var subCategory in category.SubCategories)
            {
                subCategory.CategoryImage = baseUrl + subCategory.CategoryImage.Replace("wwwroot", "");
            }
            return Ok(category);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteParentCategory(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Category Id Should Be Morethan Zero" });
            bool result = await categoryService.DeleteParentCategory(id);
            if (result)
            {
                return Ok(new { Message = "Parent Category Successfully Deleted" });
            }
            else
            {
                return BadRequest(new { Message = "Not Deleted, Please Delete Child Categories First" });
            }
        }
        [HttpDelete("DeleteChildCategory/{id}")]
        public async Task<ActionResult> DeleteChildCategory(int id)
        {
            if (id <= 0)
                return BadRequest(new { Message = "Category Id Should Be Morethan Zero" });
            bool result = await categoryService.DeleteChildCategory(id);
            if (result)
            {
                return Ok(new { Message = "Child Category Successfully Deleted" });
            }
            else
            {
                return BadRequest(new { Message = "Not Deleted, Please Delete Products Related First" });
            }
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> AddCategory([FromForm] InsertCategoryDTO insertCategoryDTO)
        {
            if (!ModelState.IsValid || insertCategoryDTO == null)
                return BadRequest();

            bool isInserted = await categoryService.AddCategory(insertCategoryDTO);
            if (isInserted)
            {
                return Ok(new { Message = "Category Inserted Successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed To Insert New Category" });
            }
        }
        [HttpPost("UpdateCategory")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateCategory([FromForm] UpdateParentCategoryDTO updateCategoryDTO)
        {
            if (!ModelState.IsValid || updateCategoryDTO == null)
                return BadRequest();
            bool isUpdated = await categoryService.UpdateCategory(updateCategoryDTO);
            if (isUpdated)
            {
                return Ok(new { Message = "Category Updated Successfully" });
            }
            else
            {
                return BadRequest(new { Message = "Failed To Update Category" });
            }
        }
        [HttpGet("GetCategoryTotalPages/{pgSize}")]
        public async Task<ActionResult> GetCategoryTotalPages(int pgSize)
        {
            if(pgSize <= 0 )
                return BadRequest(new {Message = "Page Size should be morethan Zero"});
            int result = await categoryService.GetTotalPages(pgSize);
            if(result > 0)
            {
                return Ok(result);
            }
            else
            {
                return Ok(0);
            }
        }
        #endregion
    }
}
