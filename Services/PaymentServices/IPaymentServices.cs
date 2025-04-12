using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<CartPaymentDTO?> CreateOrUpdatePayment(int basketId, string userId);
        Task<OrderHeader?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);

    }
}
