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
        private readonly BlinkDbContext _blinkDbContext;
        private readonly BrandService brandService;

        public BrandController(BrandService _brandService, BlinkDbContext blinkDbContext)
        {
            brandService = _brandService;
            _blinkDbContext = blinkDbContext;
        }

        // display all brands :
        [HttpGet("GetAllBrands")]
        public async Task<ActionResult> GetAllBrands()
        {
            var brands = await brandService.GetAllBrands();
            if (brands == null)
                return NotFound();
            return Ok(brands);
        }

        // get brand by id :
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var branch = await brandService.GetBrandbyId(id);
            if (branch == null)
                return NotFound(new ApiResponse(404, "Brand is Not Found"));
            return Ok(branch);
        }

        // get brand by name :
        [HttpGet("GetBrandByName/{name}")]
        public async Task<ActionResult> GetByName(string name)
        {
            var brands = await brandService.GetBrandByName(name);
            if (brands == null || !brands.Any()) 
                return NotFound(new ApiResponse(404, "No brands found"));
            return Ok(brands);
        }

        // insert brand 
        [HttpPost("InsertBrand")]
        public async Task<ActionResult> InsertBrand([FromBody] insertBrandDTO newbrand)
        {
             
            var brand = await brandService.InsertBrand(newbrand);

            return Ok(brand);
        }

        // update brand :
        
        [HttpPut("UpdateBrand/{id}")]
        public async Task<ActionResult> UpdateBrand(int id, [FromBody] insertBrandDTO updatebrand  )
        {
            var currentBrand = await _blinkDbContext.Brands.FindAsync(id);
            if (currentBrand == null || currentBrand.IsDeleted)
            {
                return BadRequest();
            }
            currentBrand.BrandName = updatebrand.BrandName;
            currentBrand.BrandDescription = updatebrand.BrandDescription;
            currentBrand.BrandImage = updatebrand.BrandImage;
            currentBrand.BrandWebSiteURL = updatebrand.BrandWebSiteURL;
            _blinkDbContext.Brands.Update(currentBrand);

            await _blinkDbContext.SaveChangesAsync();

            return Ok(new
            {
                message = "Brand updated successfully.",
                data = updatebrand
            });
        }
        // soft delete brand :
        [HttpDelete("SoftDeleteBrand/{id}")]
        public async Task<ActionResult> SoftDeleteBrand(int id)
        {
            var result = await brandService.SoftDeleteBrand(id);
            if (result.StatusCode == 404)
            {
                return NotFound(new ApiResponse(404, "Branch Not Found"));
            }
            return Ok(result);
        }

    }
}
