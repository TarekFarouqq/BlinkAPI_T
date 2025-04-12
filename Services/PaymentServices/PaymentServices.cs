using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stripe;

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



        public async Task<CartPaymentDTO?> CreateOrUpdatePayment(int cartId,string userId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];


            ///if (_cache.TryGetValue(cartId, out CartPaymentDTO cachedCart))
            ///{
            ///    return cachedCart; 
            ///}
            // get Cart 
            var cart = await  _unitOfWork.CartRepo.GetByUserId(userId);
            _unitOfWork.Context.Entry(cart!).State = EntityState.Detached;



            #region Convert to dto
            var mappedCartDTTo = _mapper.Map<ReadCartDTO>(cart);
            var mappedBasket =_mapper.Map<CartPaymentDTO>(mappedCartDTTo);

            #endregion

            ///get cost from DeliveryMethodRepo but check if deliveryMethodId in basket .hasvalue
            /// should have as another class
            ///if (cart.DeliveryMethodId.HasValue)
            ///{
            ///    var deliveryMethod = await _unitOfWork.Repository<DeliveryMethod>().GetByAsync(cart.DeliveryMethodId.Value);
            ///    cart.ShippingPrice = deliveryMethod.Cost;
            ///    shippingPrice = deliveryMethod.Cost;
            ///}


            var paymentIntentService = new PaymentIntentService();
            PaymentIntent paymentIntent;

            if (string.IsNullOrEmpty(mappedBasket.PaymentIntentId))
            { 
            
            
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = 1000,// this will be handle
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);

                mappedBasket.PaymentIntentId = paymentIntent.Id;
                mappedBasket.ClientSecret = paymentIntent.ClientSecret;

            }
            else { 
                var updateOptions = new PaymentIntentUpdateOptions()
                {
                    Amount = 10000 //this will be handle
                };
                await paymentIntentService.UpdateAsync(mappedBasket.PaymentIntentId, updateOptions);

            }
            await _unitOfWork.CartRepo.UpdateCart(cart);
            return mappedBasket;
        }



        public async Task<OrderHeader?> UpdatePaymentIntentToSucceededOrFailed(string paymentIntentId, bool isSucceeded)
        {
            var orderList = await _unitOfWork.OrderRepo.GetAll(); 

            var order = orderList
                .Where(o => Convert.ToString(o.PaymentId) == paymentIntentId)
                .FirstOrDefault();

            if (order == null) return null;

            if (isSucceeded)
                order.OrderStatus = "PaymentReceived";
            else
                order.OrderStatus = "PaymentFailed";

            _unitOfWork.OrderRepo.Update(order);
            await _unitOfWork.CompleteAsync();
            return order;
        }




    }



}

