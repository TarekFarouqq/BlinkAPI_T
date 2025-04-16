using Blink_API.DTOs.OrdersDTO;
using Blink_API.Errors;
using Blink_API.Models;

namespace Blink_API.Services.OrderServicees
{
    public interface IOrderServices
    {
        
        Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDTO createOrderDTO);


        Task<OrderToReturnDto> GetOrderByIdAsync(int orderId);

      

        Task<bool> DeleteOrderAsync(int orderId);

    }
}
