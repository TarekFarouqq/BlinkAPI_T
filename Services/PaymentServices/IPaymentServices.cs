<<<<<<< HEAD
<<<<<<< HEAD
﻿using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;
=======
﻿using Blink_API.DTOs.PaymentCart;
>>>>>>> 7c1b2dc (create PAyment f)
=======
﻿using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;
>>>>>>> 256852f (Create PAymeeent)
using Blink_API.Models;

namespace Blink_API.Services.PaymentServices
{
    public interface IPaymentServices
    {
        Task<CartPaymentDTO?> CreateOrUpdatePayment(int basketId, string userId);
<<<<<<< HEAD
<<<<<<< HEAD
        Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
=======
        Task<OrderHeader?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
>>>>>>> 7c1b2dc (create PAyment f)
=======
        Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded);
>>>>>>> 256852f (Create PAymeeent)

    }
}
