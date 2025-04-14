<<<<<<< HEAD
﻿using Blink_API.DTOs.CartDTOs;
using Blink_API.Models;
=======
﻿using Blink_API.Models;
>>>>>>> 7c1b2dc (create PAyment f)
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.PaymentCart
{
    public class CartPaymentDTO // Customer Cart 
    {
        public string UserId { get; set; }
        public int CartId { get; set; }
<<<<<<< HEAD
        public string PaymentMethod { get; set; }
        public string PaymentStatus { get; set; }
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }
        public decimal SubTotal { get; set; }
        public List<CartDetailsDTO> Items { get; set; }
=======
        public string PaymentMethod { get; set; }  
        public string PaymentStatus { get; set; }  
        public string PaymentIntentId { get; set; }
        public string ClientSecret { get; set; }
        public decimal ShippingPrice { get; set; }
>>>>>>> 7c1b2dc (create PAyment f)

    }
  
}

