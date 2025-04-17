
﻿using AutoMapper;
using Blink_API.Models;
using Blink_API;
using Blink_API.Services;
using Blink_API.Services.OrderServicees;
using Blink_API.DTOs.OrdersDTO;
using Blink_API.Services.UserService;
using Blink_API.Services.InventoryService;
using Blink_API.Errors;
using System.Runtime.CompilerServices;
using Blink_API.Services.Helpers;
using Blink_API.Services.PaymentServices;
public class orderService :IOrderServices
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly InventoryService _inventoryService;
    private readonly PaymentServices _paymentServices;

    public orderService(UnitOfWork unitOfWork, IMapper mapper,InventoryService inventoryService,PaymentServices paymentServices)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
       _inventoryService = inventoryService;
        _paymentServices = paymentServices;
    }


    #region  Abdelazez FInish Order 




    public async Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    {
        var cart = await _unitOfWork.CartRepo.GetByUserId(createOrderDTO.UserId);
        if (cart == null || !cart.CartDetails.Any())
            throw new Exception("Cart is empty or not found.");

        List<OrderDetail> orderDetails = new List<OrderDetail>();

        foreach (var cartDetail in cart.CartDetails)
        {
            if (cartDetail.Product == null)
                throw new Exception($"Product with ID {cartDetail.ProductId} not found in cart.");

            var productId = cartDetail.ProductId;
            var quantity = cartDetail.Quantity;

            var inventories = await _unitOfWork.StockProductInventoryRepo.GetAvailableInventoriesForProduct(productId);

            var sortedInventories = inventories
                .Where(i => i.StockQuantity > 0)
                .OrderBy(i => Helper.CalculateDistance(
                    createOrderDTO.Lat,
                    createOrderDTO.Long,
                    i.Inventory.Lat,
                    i.Inventory.Long))
                .ToList();

            int remainingQuantity = quantity;
            decimal totalPrice = 0;
            int totalTaken = 0;

            foreach (var inventory in sortedInventories)
            {
                if (remainingQuantity <= 0)
                    break;

                int takeQty = Math.Min(inventory.StockQuantity, remainingQuantity);
                inventory.StockQuantity -= takeQty;
                totalPrice += takeQty * inventory.StockUnitPrice;
                totalTaken += takeQty;
                remainingQuantity -= takeQty;

                if (inventory.StockQuantity <= 0)
                    inventory.IsDeleted = true;

                _unitOfWork.StockProductInventoryRepo.Update(inventory);
            }

            if (totalTaken < quantity)
                throw new Exception($"Not enough inventory available for product {productId}");

            decimal avgPrice = totalPrice / totalTaken;

            if (cart.OrderHeader == null || string.IsNullOrEmpty(cart.OrderHeader.PaymentIntentId))
            {
                await _paymentServices.CreateOrUpdatePayment(cart.CartId, cart.UserId);
                cart = await _unitOfWork.CartRepo.GetByUserId(createOrderDTO.UserId); 
            }


            orderDetails.Add(new OrderDetail
            {
                ProductId = productId,
                SellQuantity = totalTaken,
                SellPrice = avgPrice
            });

        }


        var orderHeader = new OrderHeader
        {
            CartId = cart.CartId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = "shipped",
            OrderTotalAmount = orderDetails.Sum(od => od.SellQuantity * od.SellPrice),
            Payment = new Payment
            {
                Method = createOrderDTO.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "pending",
                PaymentIntentId = cart.OrderHeader.PaymentIntentId,
            },
            PaymentIntentId = cart.OrderHeader.PaymentIntentId,
            OrderShippingCost = 10,
            OrderTax = 14,
        };

        _unitOfWork.OrderRepo.Add(orderHeader);
        await _unitOfWork.CompleteAsync();

        foreach (var detail in orderDetails)
        {
            detail.OrderHeaderId = orderHeader.OrderHeaderId;
            _unitOfWork.OrderDetailRepo.Add(detail);
        }

        await _unitOfWork.CompleteAsync();

        cart.IsDeleted = true;
        _unitOfWork.CartRepo.Update(cart);

        await _unitOfWork.UserRepo.UpdateUserAddress(createOrderDTO.UserId, createOrderDTO.Address);
        await _unitOfWork.CompleteAsync();

        await _unitOfWork.UserRepo.UpdateUserPhoneNumber(createOrderDTO.UserId, createOrderDTO.PhoneNumber);
        await _unitOfWork.CompleteAsync();

        var orderToReturn = _mapper.Map<OrderToReturnDto>(orderHeader);
        return orderToReturn;
    }


    //  get Order by Id
    public async Task<OrderToReturnDto> GetOrderByIdAsync(int orderId)
    {
        if (orderId <= 0)
            throw new Exception("Order number Encorrect");
        var order = await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(orderId);
        if (order is null)
            throw new Exception("Order Not Found");
        return _mapper.Map<OrderToReturnDto>(order);


    }



    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(orderId);

        if (order is null) return false;
        else
        {
            foreach (var item in order.OrderDetails)
            {
                item.IsDeleted = true;
                _unitOfWork.OrderDetailRepo.Update(item);
            }
            order.IsDeleted = true;
            order.OrderStatus = "cancelled";
        }
        _unitOfWork.OrderRepo.Update(order);

        var inventoryReturned = await _inventoryService.ReturnInventoryQuantityAfterOrderDelete(orderId);

        if (!inventoryReturned) return false;

        await _unitOfWork.CompleteAsync();
        return true;
    }




    #endregion



}

