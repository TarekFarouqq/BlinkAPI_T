using Blink_API.DTOs.BrandDtos;
using Blink_API.Models;
using Blink_API.Services.BrandServices;
using Microsoft.AspNetCore.Http;
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

        // insert brand 
        [HttpPost("InsertBrand")]
        public async Task<ActionResult> InsertBrand([FromBody] insertBrandDTO newbrand)
        {
             
            var brand = await brandService.InsertBrand(newbrand);
            
            return Ok($"{ brand.BrandName} brand added successfuly");
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
            if (result == false)
                return NotFound();
            return Ok("Brand deleted successfully.");
        }

    }
}
