using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<CartPaymentDTO?> CreateOrUpdatePayment(int basketId, string userId);
        Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);

    }
}
