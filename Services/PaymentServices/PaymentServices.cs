using AutoMapper;
using Blink_API.DTOs.CartDTOs;

using Blink_API.DTOs.OrdersDTO;

using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stripe;


namespace Blink_API.Services.PaymentServices
{
    public class PaymentServices : IPaymentServices
    {
        private readonly IConfiguration _configuration;

        ///private readonly Blink_API.Services.CartService.CartService _cartService;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMemoryCache _cache;
        private readonly IMapper _mapper;

        public PaymentServices(IConfiguration configuration, UnitOfWork unitOfWork
            , IMapper mapper)
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

                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);

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
                mappedBasket.PaymentIntentId = paymentIntent.Id;
                mappedBasket.ClientSecret = paymentIntent.ClientSecret;
            }


            await _unitOfWork.CartRepo.UpdateCart(cart);
            return mappedBasket;
        }


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




        

        }



    }
}
