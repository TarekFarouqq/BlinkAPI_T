using Azure;
using Blink_API.DTOs.BrandDtos;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Services.BranchServices;
using Blink_API.Services.BrandServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Brand
{
    [Route("api/[controller]")]
    [ApiController]
    public class BrandController : ControllerBase
    {
        private readonly BrandService brandService;
        public BrandController(BrandService _brandService, BlinkDbContext blinkDbContext)
        {
            brandService = _brandService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllBrands()
        {
            var brands = await brandService.GetAllBrands();
            if (brands == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach(var brand in brands)
            {
                brand.BrandImage = $"{baseUrl}{brand.BrandImage}";
            }
            return Ok(brands);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var brand = await brandService.GetBrandbyId(id);
            if (brand == null)
                return NotFound(new ApiResponse(404, "Brand is Not Found"));
 
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            brand.BrandImage = $"{baseUrl}{brand.BrandImage}";
 
            return Ok(brand);
        }
        [HttpGet("GetBrandByName/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var brands = await brandService.GetBrandByName(name);
            if (brands == null || !brands.Any()) 
                return NotFound(new ApiResponse(404, "No brands found"));
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var brand in brands)
            {
                brand.BrandImage = $"{baseUrl}{brand.BrandImage}";
            }
            return Ok(brands);
        }
        [HttpPost]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> InsertBrand([FromForm] insertBrandDTO newbrand)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (newbrand == null)
                return BadRequest();
            var brand = await brandService.InsertBrand(newbrand);
            if (brand.StatusCode != 200)
                return BadRequest(new {error="There is an error occured whily trying to insert brand"});
            return Ok(brand);
        }
        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public async Task<ActionResult> UpdateBrand(int id, [FromForm] insertBrandDTO updatebrand  )
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (updatebrand == null)
                return BadRequest();
            var brand = await brandService.UpdateBrand(id,updatebrand);
            if (brand.StatusCode !=  200)
                return BadRequest(new { error = "There is an error occured whily trying to update brand" });
            return Ok(brand);
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> SoftDeleteBrand(int id)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if(id == 0 )
                return BadRequest();
            var result = await brandService.SoftDeleteBrand(id);
            if(result.StatusCode != 200)
                return NotFound(new ApiResponse(404,"Brand Not Found"));
            return Ok(result);
        }
    }
}
