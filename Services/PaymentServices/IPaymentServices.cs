
﻿using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;

﻿using Blink_API.DTOs.PaymentCart;
using Blink_API.Errors;
using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<ApiResponse<CartPaymentDTO>> CreatePaymentIntentAsync(string userId, decimal amount);

        //Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);



    }
}
