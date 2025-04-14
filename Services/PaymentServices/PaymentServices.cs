using AutoMapper;
using Blink_API.DTOs.CartDTOs;
<<<<<<< HEAD
<<<<<<< HEAD
using Blink_API.DTOs.OrdersDTO;
=======
>>>>>>> 7c1b2dc (create PAyment f)
=======
using Blink_API.DTOs.OrdersDTO;
>>>>>>> 256852f (Create PAymeeent)
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stripe;
<<<<<<< HEAD
<<<<<<< HEAD
using Stripe.Issuing;
=======
>>>>>>> 7c1b2dc (create PAyment f)
=======
using Stripe.Issuing;
>>>>>>> 256852f (Create PAymeeent)

namespace Blink_API.Services.PaymentServices
{
    public class PaymentServices :IPaymentServices
    {
        private readonly IConfiguration _configuration;

        ///private readonly Blink_API.Services.CartService.CartService _cartService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public PaymentServices(IConfiguration configuration, UnitOfWork unitOfWork
            ,IMapper mapper)
            ///,Blink_API.Services.CartService.CartService cartService
            ///,UnitOfWork unitOfWork
            ///,Blink_API.Services.Product.ProductService productService)
        {
            _configuration = configuration;
            ///_cartService = cartService;
           _unitOfWork = unitOfWork;
            _mapper = mapper;
            ///_productService = productService;
        }

<<<<<<< HEAD
<<<<<<< HEAD
        public async Task<CartPaymentDTO?> CreateOrUpdatePayment(int cartId, string userId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
            var mappedBasket = _mapper.Map<CartPaymentDTO>(cart);

            mappedBasket.SubTotal = mappedBasket.Items.Sum(item => item.Quantity * item.ProductUnitPrice);
            mappedBasket.ShippingPrice = 10;
            var totalAmount = (long)((mappedBasket.SubTotal + mappedBasket.ShippingPrice) * 100);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntentToDelete = mappedBasket.PaymentIntentId;
            PaymentIntent? paymentIntent = null;

            bool shouldCreateNewIntent = false;

            if (!string.IsNullOrEmpty(mappedBasket.PaymentIntentId))
            {
                try
                {
                    var existingIntent = await paymentIntentService.GetAsync(mappedBasket.PaymentIntentId);

                    if (existingIntent.Status != "requires_payment_method" && existingIntent.Status != "requires_confirmation")
                    {
                        shouldCreateNewIntent = true;
                    }
                    else
                    {
                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = totalAmount
                        };
                        paymentIntent = await paymentIntentService.UpdateAsync(mappedBasket.PaymentIntentId, updateOptions);
                    }
                }
                catch
                {
                    shouldCreateNewIntent = true;
                }
            }
            else
            {
                shouldCreateNewIntent = true;
            }

            if (shouldCreateNewIntent)
            {
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = totalAmount,
=======


        public async Task<CartPaymentDTO?> CreateOrUpdatePayment(int cartId,string userId)
=======
        public async Task<CartPaymentDTO?> CreateOrUpdatePayment(int cartId, string userId)
>>>>>>> 256852f (Create PAymeeent)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
            var mappedBasket = _mapper.Map<CartPaymentDTO>(cart);

            mappedBasket.SubTotal = mappedBasket.Items.Sum(item => item.Quantity * item.ProductUnitPrice);
            mappedBasket.ShippingPrice = 10;
            var totalAmount = (long)((mappedBasket.SubTotal + mappedBasket.ShippingPrice) * 100);

            var paymentIntentService = new PaymentIntentService();
            var paymentIntentToDelete = mappedBasket.PaymentIntentId;
            PaymentIntent? paymentIntent = null;

            bool shouldCreateNewIntent = false;

            if (!string.IsNullOrEmpty(mappedBasket.PaymentIntentId))
            {
                try
                {
                    var existingIntent = await paymentIntentService.GetAsync(mappedBasket.PaymentIntentId);

                    if (existingIntent.Status != "requires_payment_method" && existingIntent.Status != "requires_confirmation")
                    {
                        shouldCreateNewIntent = true;
                    }
                    else
                    {
                        var updateOptions = new PaymentIntentUpdateOptions()
                        {
                            Amount = totalAmount
                        };
                        paymentIntent = await paymentIntentService.UpdateAsync(mappedBasket.PaymentIntentId, updateOptions);
                    }
                }
                catch
                {
                    shouldCreateNewIntent = true;
                }
            }
            else
            {
                shouldCreateNewIntent = true;
            }

            if (shouldCreateNewIntent)
            {
                var createOptions = new PaymentIntentCreateOptions()
                {
<<<<<<< HEAD
                    Amount = 1000,// this will be handle
>>>>>>> 7c1b2dc (create PAyment f)
=======
                    Amount = totalAmount,
>>>>>>> 256852f (Create PAymeeent)
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);

<<<<<<< HEAD
<<<<<<< HEAD
=======
>>>>>>> 256852f (Create PAymeeent)
                if (!string.IsNullOrEmpty(paymentIntentToDelete))
                {
                    try
                    {
                        await paymentIntentService.CancelAsync(paymentIntentToDelete);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Could not delete old PaymentIntent {paymentIntentToDelete}: {ex.Message}");
                    }
                }
            }

            if (paymentIntent != null)
            {
<<<<<<< HEAD
                mappedBasket.PaymentIntentId = paymentIntent.Id;
                mappedBasket.ClientSecret = paymentIntent.ClientSecret;
            }

=======
=======
>>>>>>> 256852f (Create PAymeeent)
                mappedBasket.PaymentIntentId = paymentIntent.Id;
                mappedBasket.ClientSecret = paymentIntent.ClientSecret;
            }

<<<<<<< HEAD
            }
>>>>>>> 7c1b2dc (create PAyment f)
=======
>>>>>>> 256852f (Create PAymeeent)
            await _unitOfWork.CartRepo.UpdateCart(cart);
            return mappedBasket;
        }


<<<<<<< HEAD
<<<<<<< HEAD
        public async Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
           
            var orderHeader = await _unitOfWork.OrderRepo
                .GetOrderByPaymentIntentId(paymentIntentId);

            if (orderHeader == null) return null;

            orderHeader.OrderStatus = isSucceeded ? "PaymentReceived" : "PaymentFailed";

            _unitOfWork.OrderRepo.Update(orderHeader);
            await _unitOfWork.CompleteAsync();

            var dto = _mapper.Map<orderDTO>(orderHeader);
            return dto;
=======

        public async Task<OrderHeader?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
=======
        public async Task<orderDTO?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
>>>>>>> 256852f (Create PAymeeent)
        {
           
            var orderHeader = await _unitOfWork.OrderRepo
                .GetOrderByPaymentIntentId(paymentIntentId);

            if (orderHeader == null) return null;

            orderHeader.OrderStatus = isSucceeded ? "PaymentReceived" : "PaymentFailed";

            _unitOfWork.OrderRepo.Update(orderHeader);
            await _unitOfWork.CompleteAsync();
<<<<<<< HEAD
            return order;
>>>>>>> 7c1b2dc (create PAyment f)
=======

            var dto = _mapper.Map<orderDTO>(orderHeader);
            return dto;
>>>>>>> 256852f (Create PAymeeent)
        }




<<<<<<< HEAD
<<<<<<< HEAD


=======
>>>>>>> 7c1b2dc (create PAyment f)
=======


>>>>>>> 256852f (Create PAymeeent)
    }



}

