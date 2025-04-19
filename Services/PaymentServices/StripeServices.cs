using AutoMapper;
using Blink_API.DTOs.OrdersDTO;
using Blink_API.Errors;
using Stripe;

namespace Blink_API.Services.PaymentServices
{
    public class StripeServices
    {
        private readonly IConfiguration _configuration;
        private readonly UnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly orderService _orderService;

        public StripeServices(IConfiguration configuration, UnitOfWork unitOfWork
            , IMapper mapper , orderService orderService) 
        {
            _configuration = configuration;
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _orderService = orderService;
        }


        public async Task<ApiResponse<OrderToReturnDto>> ConfirmPaymentAndCreateOrderAsync(string userId, string paymentIntentId, CreateOrderDTO createOrderDTO)
        {
            try
            {
                var isSucceeded = await PollPaymentStatus(paymentIntentId);
                if (!isSucceeded)
                {
                    return new ApiResponse<OrderToReturnDto>(400, "Payment failed. Cannot create order.");
                }

                var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
                if (cart == null || !cart.CartDetails.Any())
                {
                    return new ApiResponse<OrderToReturnDto>(404, "Cart is empty or not found.");
                }

                var payment = await _unitOfWork.PaymentRepository.GetPaymentByIntentId(paymentIntentId);
                if (payment == null)
                {
                    return new ApiResponse<OrderToReturnDto>(404, "Payment not found.");
                }

                payment.PaymentStatus = "succeeded";
                _unitOfWork.PaymentRepository.Update(payment);
                await _unitOfWork.CompleteAsync();

                var order = await _orderService.CreateOrderInternalAsync(cart, createOrderDTO, payment);
                return new ApiResponse<OrderToReturnDto>(200, "Order created successfully.", order);
            }
            catch (Exception ex)
            {
                return new ApiResponse<OrderToReturnDto>(500, $"Something went wrong: {ex.Message}");
            }
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
        public async Task<bool> PollPaymentStatus(string paymentIntentId)
        {
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

            var paymentIntentService = new PaymentIntentService();

            try
            {
                var paymentIntent = await paymentIntentService.GetAsync(paymentIntentId);


                if (paymentIntent.Status == "succeeded")
                {
                    return true;
                }

                else if (paymentIntent.Status == "failed")
                {
                    return false;
                }
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error occurred: {ex.Message}");
            }

            return false;
        }

    }
}
