using Blink_API.DTOs.PaymentCart;
using Blink_API.Errors;
using Blink_API.Services.PaymentServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Payment
{
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServices _paymentServices;
        private readonly UnitOfWork _unitOfWork;

        public PaymentController(PaymentServices paymentServices,UnitOfWork  unitOfWork )
        {
            _paymentServices = paymentServices;
            _unitOfWork = unitOfWork;
        }

        [HttpPost]
        public async Task<ActionResult<CartPaymentDTO>> CreateOrUpdatePaymentIntent(CartPaymentDTO cartPayment)
        {
            try
            {
                if (string.IsNullOrEmpty(cartPayment.PaymentMethod))
                {
                    return NotFound(new ApiResponse(404, "Payment method is required."));
                }

                if (cartPayment == null) return NotFound(new ApiResponse(404, "Cart Not Found"));

                var userId = cartPayment.UserId;
                if (string.IsNullOrEmpty(userId))
                {
                    return BadRequest(new ApiResponse(400, "User ID is missing"));
                }

                var basket = await _paymentServices.CreateOrUpdatePayment(cartPayment.CartId, userId);

                if (basket is null) return BadRequest(new ApiResponse(400, "An Error With your Cart!"));
                return Ok(basket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "Internal Server Error: " + ex.Message));
            }
        }





    }
}
