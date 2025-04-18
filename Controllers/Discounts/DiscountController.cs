using Blink_API.DTOs.DiscountDTO;
using Blink_API.Services.DiscountServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Discounts
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiscountController : ControllerBase
    {
        private readonly DiscountService discountService;
        public DiscountController(DiscountService _discountService)
        {
            discountService = _discountService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllRunningDiscounts()
        {
            var discounts = await discountService.GetRunningDiscounts();
            if (discounts == null) return NotFound();
            return Ok(discounts);
        }
        [HttpGet("{DiscountId}")]
        public async Task<ActionResult> GetRunningDiscountById(int DiscountId)
        {
            var discount = await discountService.GetRunningDiscountById(DiscountId);
            if (discount == null) return NotFound();
            return Ok(discount);
        }
        [HttpGet("GetDiscounts")]
        public async Task<ActionResult> GetAll()
        {
            return Ok(await discountService.GetAllDiscounts());
        }
        [HttpGet("GetDiscountById/{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            return Ok(await discountService.GetDiscountById(id));
        }
        [HttpPost]
        public async Task<ActionResult> CreateDiscount(InsertDiscountDetailsDTO insertDiscountDetailsDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (insertDiscountDetailsDTO == null)
                return BadRequest();
            await discountService.CreateDiscount(insertDiscountDetailsDTO);
            return Ok();
        }
        [HttpPut]
        public async Task<ActionResult> UpdateDiscount(UpdateDiscountDetailsDTO updateDiscountDetailsDTO)
        {
            if(!ModelState.IsValid)
                return BadRequest();
            if(updateDiscountDetailsDTO == null)
                return BadRequest();
            await discountService.UpdateDiscount(updateDiscountDetailsDTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteDiscount(int id)
        {
            if(id <= 0)
                return BadRequest();
            await discountService.DeleteDiscount(id);
            return Ok();
        }
    }
}
