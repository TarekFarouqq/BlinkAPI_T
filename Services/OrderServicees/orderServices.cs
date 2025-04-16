
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
public class orderService :IOrderServices
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly InventoryService _inventoryService;

    public orderService(UnitOfWork unitOfWork, IMapper mapper,InventoryService inventoryService)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
       _inventoryService = inventoryService;
    }


    #region  Abdelazez FInish Order 




    public async Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    {
        // Get Cart + Cart Details + Product + StockProductInventories
        var cart = await _unitOfWork.CartRepo.GetByUserId(createOrderDTO.UserId);
        if (cart == null || !cart.CartDetails.Any())
            throw new Exception("Cart is empty or not found.");

        // prepare List of OrderDetails
        List<OrderDetail> orderDetails = new List<OrderDetail>();

        // looping on cartDetails
        foreach (var cartDetail in cart.CartDetails)
        {
            if (cartDetail.Product == null)
                throw new Exception($"Product with ID {cartDetail.ProductId} not found in cart.");

            var productId = cartDetail.ProductId;
            var quantity = cartDetail.Quantity;
            //var inventories = cartDetail.Product.StockProductInventories
            //    .Where(sp => !sp.IsDeleted && sp.ProductId == productId);
            var inventories = await _unitOfWork.StockProductInventoryRepo.GetAvailableInventoriesForProduct(productId);



            var closestInventory = inventories
                .Where(i => i.StockQuantity >= quantity)
                .OrderBy(i => Helper.CalculateDistance(createOrderDTO.Lat, createOrderDTO.Long, i.Inventory.Lat, i.Inventory.Long))
                .FirstOrDefault();

            if (closestInventory == null)
                throw new Exception($"No available inventory found for product {productId}");

            closestInventory.StockQuantity -= quantity;
            if (closestInventory.StockQuantity <= 0)
            {
                closestInventory.IsDeleted = true;
            }
            _unitOfWork.StockProductInventoryRepo.Update(closestInventory);
            decimal avgPrice = inventories.Any() ? inventories.Average(s => s.StockUnitPrice) : 0;

            orderDetails.Add(new OrderDetail
            {
                ProductId = productId,
                SellQuantity = quantity,
                SellPrice = avgPrice
            });

            // Update StockProductInventory
            var inventoryItem = inventories.FirstOrDefault();
            if (inventoryItem != null)
            {
                inventoryItem.StockQuantity -= quantity;

                if (inventoryItem.StockQuantity <= 0)
                {
                    inventoryItem.IsDeleted = true;
                }

                _unitOfWork.StockProductInventoryRepo.Update(inventoryItem);
            }
        }

        // Create OrderHeader
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
                PaymentIntentId = "0"
            },
            PaymentIntentId = "0",
            OrderShippingCost = 10,
            OrderTax = 14,
        };

        // Save OrderHeader
        _unitOfWork.OrderRepo.Add(orderHeader);
        await _unitOfWork.CompleteAsync();

        // Assign OrderHeaderId to OrderDetails
        foreach (var detail in orderDetails)
        {
            detail.OrderHeaderId = orderHeader.OrderHeaderId;
            _unitOfWork.OrderDetailRepo.Add(detail);
        }

        await _unitOfWork.CompleteAsync();

        // Clear Cart 
        cart.IsDeleted = true;
        _unitOfWork.CartRepo.Update(cart);

        // Update User Address
        await _unitOfWork.UserRepo.UpdateUserAddress(createOrderDTO.UserId, createOrderDTO.Address);
        await _unitOfWork.CompleteAsync();

        // Update UserPhoneNumber
        await _unitOfWork.UserRepo.UpdateUserPhoneNumber(createOrderDTO.UserId,createOrderDTO.PhoneNumber);
        await _unitOfWork.CompleteAsync();

        // Map OrderHeader to OrderToReturnDto
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

