using System.Linq;
using AutoMapper;
using Blink_API.DTOs.CartDTOs;

using Blink_API.DTOs.OrdersDTO;

using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Stripe;
using Stripe.Issuing;


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
        public async Task<CartPaymentDTO?> CreateOrUpdatePayment2(int cartId,string userId)
        {
            // Set the Stripe API Key from configuration
            StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];
            var UserCart = await _unitOfWork.CartRepo.GetByUserId(userId);
            if (UserCart == null)
                throw new BadHttpRequestException("Cart Not Found");
            // Initialize the PaymentIntent service and payment intent variable
            var paymentIntentService = new PaymentIntentService();
            PaymentIntent? paymentIntent = null;
            // Flag to check whether a new payment intent should be created
            bool shouldCreateNewIntent = false;
            // Check if the cart does not have an OrderHeader, then create one
            if (UserCart.OrderHeader == null)
            {
                OrderHeader newOrder = new OrderHeader();
                newOrder.OrderDate = DateTime.UtcNow; // ----------------- 1
                var OrderSubTotal = UserCart.CartDetails.Sum(cd => cd.Product.StockProductInventories.Average(spi => spi.StockUnitPrice)); // ----------------- 2
                var OrderTax = OrderSubTotal * 0.14m; // ----------------- 1
                var ShippingCost = 0;
                var OrderTotalAmount = OrderSubTotal + OrderTax + ShippingCost;
                var OrderStatus = "shipped";
                ICollection<OrderDetail> orderDetails = new HashSet<OrderDetail>();
                foreach (var orderDetail in UserCart.CartDetails)
                {
                    orderDetails.Add(new OrderDetail
                    {
                        SellQuantity = orderDetail.Quantity,
                        SellPrice = UserCart.CartDetails.FirstOrDefault(p => p.ProductId == orderDetail.ProductId).Product.StockProductInventories.Average(spi => spi.StockUnitPrice),
                        ProductId = orderDetail.ProductId,
                        OrderHeader = newOrder
                    });
                }
                var listOfOrderDetails = orderDetails.ToList();
                newOrder.OrderSubtotal = OrderSubTotal;
                newOrder.OrderTax = OrderTax;
                newOrder.OrderShippingCost = ShippingCost;
                newOrder.OrderTotalAmount = OrderTotalAmount;
                newOrder.OrderStatus = OrderStatus;
                newOrder.OrderDetails = orderDetails.ToList();
                // Create Payment Method
                Payment newPayment = new Payment();
                newPayment.Method = "Card";
                newPayment.PaymentStatus = "Pending";
                newPayment.PaymentDate = DateTime.UtcNow;
                // Create a new PaymentIntent if necessary
                var createOptions = new PaymentIntentCreateOptions()
                {
                    Amount = (long)OrderTotalAmount,
                    Currency = "usd",
                    PaymentMethodTypes = new List<string> { "card" }
                };
                paymentIntent = await paymentIntentService.CreateAsync(createOptions);
                newPayment.PaymentIntentId = paymentIntent.Id;
                newPayment.OrderHeader = newOrder;
                newOrder.Payment = newPayment;
                newOrder.PaymentIntentId = paymentIntent.Id;
                newOrder.CartId = cartId;
                newOrder.PaymentIntentId = paymentIntent.Id;
                // Map the cart to CartPaymentDTO and add the ClientSecret to the response DTO
                var mappedCart = _mapper.Map<CartPaymentDTO>(UserCart);
                mappedCart.ClientSecret = paymentIntent.ClientSecret;
                mappedCart.PaymentIntentId = paymentIntent.Id;
                mappedCart.SubTotal = OrderSubTotal;
                mappedCart.TotalAmount= OrderTotalAmount;
                var mappedCartDetailsDTO = _mapper.Map<List<CartDetailsDTO>>(UserCart.CartDetails);
                mappedCart.Items = mappedCartDetailsDTO;
                // Update the cart in the database
                await _unitOfWork.CartRepo.UpdateCart(UserCart);
                // Return the mapped cart with payment information
                return mappedCart;
            }
            return new CartPaymentDTO();
        }
        public async Task<CartPaymentDTO?> CreateOrUpdatePayment(int cartId, string userId)
        {
            try
            {
                // Set the Stripe API Key from configuration
                StripeConfiguration.ApiKey = _configuration["StripeSettings:SecretKey"];

                // Retrieve the cart for the user
                var cart = await _unitOfWork.CartRepo.GetByUserId(userId);

                if (cart is null)
                    throw new BadHttpRequestException("Cart Not Found");

                // Initialize the PaymentIntent service and payment intent variable
                var paymentIntentService = new PaymentIntentService();
                PaymentIntent? paymentIntent = null;

                // Flag to check whether a new payment intent should be created
                bool shouldCreateNewIntent = false;

                // Check if the cart does not have an OrderHeader, then create one
                if (cart.OrderHeader == null)
                {
                    cart.OrderHeader = new OrderHeader();

                    // Calculate the subtotal of the order by iterating through cart details
                    decimal subtotal = 0;

                    foreach (var detail in cart.CartDetails)
                    {
                        if (detail.Product == null) continue;

                        // Get all available stock inventories for the product
                        var inventories = await _unitOfWork.StockProductInventoryRepo.GetAvailableInventoriesForProduct(detail.ProductId);

                        int remaining = detail.Quantity; //Sold Quantity
                        decimal totalPrice = 0; // 
                        int totalTaken = 0;


                        var averagePrice = inventories.Average(i => i.StockUnitPrice);

                        // Iterate through the inventories and calculate the total price based on the available stock
                        foreach (var inventory in inventories)
                        {
                            if (remaining <= 0) break;

                            int takeQty = Math.Min(remaining, inventory.StockQuantity); // Take the available quantity
                            totalPrice += takeQty * inventory.StockUnitPrice; // Add the price to the total
                            totalTaken += takeQty; // Keep track of the total taken quantity
                            remaining -= takeQty; // Subtract the taken quantity from remaining
                        }

                        // If not enough stock is available, throw an exception
                        if (totalTaken < detail.Quantity)
                            throw new Exception("Not enough stock for product.");

                        // Calculate the average price for the taken quantity
                        decimal avgPrice = totalPrice / totalTaken;

                        // Add the product's total price to the subtotal
                        subtotal += avgPrice * totalTaken;
                    }

                    // Assign the calculated values to the OrderHeader
                    cart.OrderHeader.OrderSubtotal = subtotal; // Save subtotal to OrderSubtotal
                    cart.OrderHeader.OrderShippingCost = 10; // Shipping cost
                    cart.OrderHeader.OrderTax = 14; // Tax amount
                }

                // Check if the cart has a PaymentIntentId and try to reuse or update the existing payment intent
                if (!string.IsNullOrEmpty(cart.OrderHeader?.PaymentIntentId))
                {
                    try
                    {
                        // Retrieve the existing PaymentIntent
                        var existingIntent = await paymentIntentService.GetAsync(cart.OrderHeader.PaymentIntentId);

                        // Check if the PaymentIntent is not in a valid state for reuse
                        if (existingIntent.Status != "requires_payment_method" && existingIntent.Status != "requires_confirmation")
                        {
                            shouldCreateNewIntent = true; // Create a new PaymentIntent if the current one is invalid
                        }
                        else
                        {
                            // Update the existing PaymentIntent with the new amount
                            var updateOptions = new PaymentIntentUpdateOptions()
                            {
                                Amount = 10000,
                            };
                            paymentIntent = await paymentIntentService.UpdateAsync(cart.OrderHeader.PaymentIntentId, updateOptions);
                        }
                    }
                    catch
                    {
                        shouldCreateNewIntent = true; // If an error occurs, create a new PaymentIntent
                    }
                }
                else
                {
                    shouldCreateNewIntent = true; // If no PaymentIntent exists, create a new one
                }

                // Calculate the total amount (including subtotal, shipping, and tax)
                decimal totalAmount = cart.OrderHeader.OrderTotalAmount;
                var stripeAmount = (long)(totalAmount * 100); // Convert the total amount to cents for Stripe

                // Create a new PaymentIntent if necessary
                if (shouldCreateNewIntent)
                {
                    var createOptions = new PaymentIntentCreateOptions()
                    {
                        Amount = 100000, // will by calculated
                        Currency = "usd", // Currency for the payment
                        PaymentMethodTypes = new List<string> { "card" } // Supported payment methods
                    };
                    paymentIntent = await paymentIntentService.CreateAsync(createOptions);
                }

                // If a payment intent was successfully created or updated, save the PaymentIntentId and ClientSecret
                if (paymentIntent != null)
                {
                    cart.OrderHeader.PaymentIntentId = paymentIntent.Id;
                    // cart.OrderHeader.ClientSecret = paymentIntent.ClientSecret; // Save the ClientSecret for front-end use
                }

                // Map the cart to CartPaymentDTO and add the ClientSecret to the response DTO
                var mappedCart = _mapper.Map<CartPaymentDTO>(cart);
                mappedCart.ClientSecret = paymentIntent.ClientSecret;

                // Update the cart in the database
                await _unitOfWork.CartRepo.UpdateCart(cart);

                // Return the mapped cart with payment information
                return mappedCart;
            }
            catch (Exception ex)
            {
                throw new Exception($"Order creation failed: {ex.Message}", ex);
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

        public async Task MonitorPaymentStatus(string paymentIntentId)
        {
            bool isPaid = false;

            for (int i = 0; i < 3; i++)
            {
                isPaid = await PollPaymentStatus(paymentIntentId);

                if (isPaid)
                {
                    break;
                }

                await Task.Delay(TimeSpan.FromMinutes(5));
            }

            if (isPaid)
            {
                await UpdatePaymentIntentToSucceededOrFailed(paymentIntentId, true);
            }
            else
            {
                await UpdatePaymentIntentToSucceededOrFailed(paymentIntentId, false);
            }
        }

        public async Task<CartPaymentDTO> HandlePaymentAsync(int cartId, string userId)
        {
            var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
            var paymentIntentId = cart.OrderHeader.PaymentIntentId;

            await MonitorPaymentStatus(paymentIntentId);

            var cartPaymentDTO = _mapper.Map<CartPaymentDTO>(cart);
            return cartPaymentDTO;
        }



    }
}
