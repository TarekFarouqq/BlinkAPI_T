using Blink_API.DTOs.OrdersDTO;
using Blink_API.Errors;
using Blink_API.Models;

namespace Blink_API.Services.OrderServicees
{
    public interface IOrderServices
    {
        
        Task<ApiResponse<OrderToReturnDto>> CreateOrderAsync(CreateOrderDTO createOrderDTO);


        Task<orderDTO> GetOrderByIdAsync(int orderId);

        Task<List<orderDTO>> GetOrdersByUserIdAsync(string userId);


        Task<bool> DeleteOrderAsync(int orderId);

    }
}
