using Blink_API.DTOs.OrdersDTO;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Services.Helpers;
using Blink_API.Services.PaymentServices;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Blink_API.Controllers.Order
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly orderService _orderService;
        private readonly UnitOfWork _unitOfWork;

        public OrderController(orderService orderService, UnitOfWork unitOfWork)
        {
            _orderService = orderService;
            _unitOfWork = unitOfWork;
        }

        [HttpPost("create")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateOrder([FromBody] CreateOrderDTO orderDTO)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(new ApiResponse(400, "Invalid request data"));

                var order = await _orderService.CreateOrderAsync(orderDTO);

                if (order == null)
                    return BadRequest(new ApiResponse(400, "Failed to create order"));

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "Internal Server Error: " + ex.Message));
            }
        }

        [HttpGet("{orderId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetOrderById(int orderId)
        {
            try
            {
                var order = await _orderService.GetOrderByIdAsync(orderId);
                if (order == null)
                    return NotFound(new ApiResponse(404, "Order Not Found"));

                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse(500, "Internal Server Error: " + ex.Message));
            }
        }

        [HttpGet("GetOrdersByUserId")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<orderDTO>>> GetOrdersByUserIdAsync()
        {
            try
            {
                var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

                if (string.IsNullOrEmpty(userId))
                {
                    return Unauthorized(new ApiResponse(401, "User is not authenticated"));
                }

                var orders = await _orderService.GetOrdersByUserIdAsync(userId);
                string baseUrl = $"{Request.Scheme}://{Request.Host}/";
                foreach (var order in orders)
                {
                    foreach (var item in order.Items)
                    {
                        int startIndex = item.ProductImageUrl.IndexOf("/images/");
                        item.ProductImageUrl = baseUrl + item.ProductImageUrl.Substring(startIndex);
                        //item.ProductImageUrl = item.ProductImageUrl.Select(img => $"{baseUrl}{item.ProductImageUrl.Replace("wwwroot/", "")}").ToList();

                    }
                }
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return BadRequest(new ApiResponse(400, "Error: " + ex.Message));
            }
        }

        [HttpDelete("{orderId}")]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteOrder(int orderId)
        {
            var success = await _orderService.DeleteOrderAsync(orderId);

            if (!success)
                return NotFound(new ApiResponse(404, "Order not found or could not be deleted"));

            return Ok(new ApiResponse(200, "Order deleted and inventory quantities restored"));
        }
    }
}
