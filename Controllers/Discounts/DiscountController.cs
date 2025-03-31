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
    }
}
