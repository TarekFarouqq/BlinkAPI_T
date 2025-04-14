<<<<<<< HEAD
﻿using AutoMapper;
using Blink_API.Models;
using Blink_API;
using Blink_API.Services;
public class orderService 
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public orderService(UnitOfWork unitOfWork, IMapper mapper)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    /////public async Task<orderDTO?> CreateOrderAsync(string userId)
    /////{
    /////    var cart = await _unitOfWork.CartRepo.GetByUserId(userId);
    /////    if (cart == null || cart.CartDetails.Count == 0) return null;
    ///
    /////    var payment = await _unitOfWork.PaymentRepo.GetByPaymentIntentId(cart.OrderHeader.PaymentIntentId);
    /////    if (payment == null) return null;
    ///
    /////    var order = new OrderHeader
    /////    {
    /////        OrderDate = DateTime.UtcNow,
    /////        OrderStatus = "Pending",
    /////        OrderSubtotal = cart.CartDetails.Sum(i => i.Quantity * i.Product.),
    /////        OrderShippingCost = cart.ShippingPrice,
    /////        OrderTax = 0, // أو تحسب الضريبة حسب اللوجيك بتاعك
    /////        OrderTotalAmount = cart.CartItems.Sum(i => i.Quantity * i.Product.GetCurrentPrice()) + cart.ShippingPrice,
    /////        PaymentIntentId = cart.PaymentIntentId,
    /////        PaymentId = payment.PaymentId,
    /////        CartId = cart.CartId
    /////    };
    ///
    /////    foreach (var item in cart.CartItems)
    /////    {
    /////        order.OrderDetails.Add(new OrderDetail
    /////        {
    /////            ProductId = item.ProductId,
    /////            SellQuantity = item.Quantity,
    /////            SellPrice = item.Product.GetCurrentPrice()
    /////        });
    /////    }
    ///
    /////    await _unitOfWork.OrderRepo.AddAsync(order);
    /////    await _unitOfWork.CompleteAsync();
    ///
    /////    return _mapper.Map<orderDTO>(order);
    /////}
    ///
    /////public async Task<orderDTO?> GetOrderByPaymentIntentIdAsync(string paymentIntentId)
    /////{
    /////    var order = await _unitOfWork.OrderRepo.GetOrderByPaymentIntentId(paymentIntentId);
    /////    return order != null ? _mapper.Map<orderDTO>(order) : null;
    /////}
    ///
    /////public async Task<IEnumerable<orderDTO>> GetOrdersForUserAsync(string userId)
    /////{
    /////    var orders = await _unitOfWork.OrderRepo.GetOrdersByUserId(userId);
    /////    return _mapper.Map<IEnumerable<orderDTO>>(orders);
    /////}
    ///
    /////public async Task<orderDTO?> UpdatePaymentStatusAsync(string paymentIntentId, bool isSucceeded)
    /////{
    /////    var order = await _unitOfWork.OrderRepo.GetOrderByPaymentIntentId(paymentIntentId);
    /////    if (order == null) return null;
    ///
    /////    order.OrderStatus = isSucceeded ? "PaymentReceived" : "PaymentFailed";
    /////    _unitOfWork.OrderRepo.Update(order);
    /////    await _unitOfWork.CompleteAsync();
    ///
    /////    return _mapper.Map<orderDTO>(order);
    /////}




    #region Abdelazez 
    public async Task<List<OrderHeader>> GetAllOrders()
    {
        return await _unitOfWork.OrderRepo.GetOrdersWithDetails();
    }
    public async Task<OrderHeader?> GetOrderById(int id)
    {
        return await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(id);
    }

    public async Task AddOrder(OrderHeader order)
    {
        _unitOfWork.OrderRepo.Add(order);
        await _unitOfWork.CompleteAsync();
    }

    public async Task DeleteOrder(int id)
    {
        await _unitOfWork.OrderRepo.Delete(id);
        await _unitOfWork.CompleteAsync();
    }


    public async Task UpdateOrder(OrderHeader order)
    {
        _unitOfWork.OrderRepo.Update(order);
        await _unitOfWork.CompleteAsync();

    }

    #endregion




}

=======
﻿using Blink_API.Models;
using Blink_API.Repositories.Order;

namespace Blink_API.Services.OrderServicees
{
    public class orderServices
    {
        private readonly UnitOfWork _unitOfWork;

        public orderServices(UnitOfWork unitOfWork)
        {
           _unitOfWork = unitOfWork;
        }

        public async Task<List<OrderHeader>> GetAllOrders()
        {
            return await _unitOfWork.OrderRepo.GetOrdersWithDetails();
        }
        public async Task<OrderHeader?> GetOrderById(int id)
        {
            return await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(id);
        }

        public async Task AddOrder(OrderHeader order)
        {
             _unitOfWork.OrderRepo.Add(order);
           await _unitOfWork.CompleteAsync();
        }

        public async Task DeleteOrder(int id)
        {
            await _unitOfWork.OrderRepo.Delete(id);
            await _unitOfWork.CompleteAsync();
        }


        public async Task UpdateOrder(OrderHeader order)
        {
            _unitOfWork.OrderRepo.Update(order);
            await _unitOfWork.CompleteAsync();

        }
    }
}
>>>>>>> 7c1b2dc (create PAyment f)
