using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Errors;
using Blink_API.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blink_API.Controllers.Payment
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class PaymentController : ControllerBase
    {
        private readonly PaymentServices _paymentServices;
        private readonly UnitOfWork _unitOfWork;
        private readonly StripeServices _stripeServices;

        public PaymentController(PaymentServices paymentServices, UnitOfWork unitOfWork, StripeServices stripeServices)
        {
            _paymentServices = paymentServices;
            _unitOfWork = unitOfWork;
            _stripeServices = stripeServices;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrUpdatePaymentIntent()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse<CartPaymentDTO>(401));
                }

                var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
                if (cart == null)
                {
                    return NotFound(new ApiResponse<CartPaymentDTO>(404, "Cart not found for the current user"));
                }

                #region Calc TOTAL 
                var subTotal = cart.CartDetails.Sum(cd => cd.Product.StockProductInventories.Average(spi => spi.StockUnitPrice));
                var tax = subTotal * 0.14m;
                var shipping = 0m;
                var total = subTotal + tax + shipping;
                #endregion

                var basket = await _paymentServices.CreatePaymentIntentAsync(userId, total);
                if (basket == null)
                {
                    return BadRequest(new ApiResponse<CartPaymentDTO>(400, "An error occurred while processing your cart"));
                }

                return Ok(basket);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<CartPaymentDTO>(500, ex.Message));
            }
        }

        [HttpPost("confirmPayment")]
        public async Task<ActionResult<ApiResponse<orderDTO>>> ConfirmPayment([FromBody] ConfirmPaymentDTO dto)
        {
            try
            {
                var order = await _stripeServices.UpdatePaymentIntentToSucceededOrFailed(dto.paymentIntentId, dto.isSucceeded);
                if (order == null)
                {
                    return NotFound(new ApiResponse<orderDTO>(404, "Order not found"));
                }

                return Ok(new ApiResponse<orderDTO>(200, "Payment confirmed", order));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<orderDTO>(500, ex.Message));
            }
        }
    }
}


//[HttpGet("status/{paymentIntentId}")]
//public async Task<IActionResult> GetPaymentStatus(string paymentIntentId)
//{
//    try
//    {
//        var createOrderDTO = new CreateOrderDTO(); 
//        await _paymentServices.MonitorPaymentStatus(paymentIntentId, createOrderDTO);

//        return Ok("Payment status updated successfully");
//    }

//    catch (Exception ex)
//    {
//        return StatusCode(500, $"Internal server error: {ex.Message}");
//    }
//}
#region create WebHook

//[HttpPost("webhook")] //api/Payment/webhook
//public async Task<IActionResult> StripeWebhook()
//{
//    var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
//    var sigHeader = Request.Headers["Stripe-Signature"];
//    Event stripeEvent;

//    try
//    {
//        // تحقق من الـ Webhook باستخدام الـ Secret Key الخاص بـ Stripe
//        stripeEvent = EventUtility.ConstructEvent(json, sigHeader, _whSecret);
//    }
//    catch (StripeException e)
//    {
//        return BadRequest($"Webhook Error: {e.Message}");
//    }

//    // هنا بنحدد التعامل مع الأحداث حسب نوع الحدث (Event)
//    switch (stripeEvent.Type)
//    {
//        case Events.PaymentIntentSucceeded:
//            var paymentIntentSucceeded = stripeEvent.Data.Object as PaymentIntent;
//            // هنا بتعمل شيء بعد نجاح الدفع
//            await HandlePaymentIntentSucceeded(paymentIntentSucceeded);
//            break;

//        case Events.PaymentIntentPaymentFailed:
//            var paymentIntentFailed = stripeEvent.Data.Object as PaymentIntent;
//            // هنا بتعمل شيء بعد فشل الدفع
//            await HandlePaymentIntentFailed(paymentIntentFailed);
//            break;

//        default:
//            // أنواع الأحداث الأخرى ممكن تضيفها هنا
//            break;
//    }

//    return Ok();  // Stripe بيحتاج الرد ده علشان يعرف إننا استقبلنا الحدث
//}

//private async Task HandlePaymentIntentSucceeded(PaymentIntent paymentIntent)
//{
//    var order = await _unitOfWork.OrderRepo.GetOrderByPaymentIntentId(paymentIntent.Id);

//    if (order != null)
//    {
//        order.Status = "PaymentReceived";
//        _unitOfWork.OrderRepo.Update(order);
//        await _unitOfWork.CompleteAsync();
//    }
//}

//private async Task HandlePaymentIntentFailed(PaymentIntent paymentIntent)
//{
//    var order = await _unitOfWork.OrderRepo.GetOrderByPaymentIntentId(paymentIntent.Id);

//    if (order != null)
//    {
//        order.Status = "PaymentFailed";
//        _unitOfWork.OrderRepo.Update(order);
//        await _unitOfWork.CompleteAsync();
//    }
//}



#endregion




