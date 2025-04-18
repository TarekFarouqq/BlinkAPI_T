
﻿using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;

﻿using Blink_API.DTOs.PaymentCart;

using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<CartPaymentDTO?> CreatePaymentIntent(string userId, decimal amount);

        Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);



    }
}
