
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
using Microsoft.EntityFrameworkCore;
public class orderService :IOrderServices
{
    private readonly UnitOfWork _unitOfWork;
    private readonly IMapper _mapper;
    private readonly InventoryService _inventoryService;
    private readonly PaymentServices _paymentServices;
    private readonly BlinkDbContext _blinkDbContext;

    public orderService(UnitOfWork unitOfWork, IMapper mapper,InventoryService inventoryService,PaymentServices paymentServices,BlinkDbContext blinkDbContext)
    {
        _unitOfWork = unitOfWork;
        _mapper = mapper;
       _inventoryService = inventoryService;
        _paymentServices = paymentServices;
       _blinkDbContext = blinkDbContext;
    }


    #region Test Finish
    // For cash payments only
    public async Task<ApiResponse<OrderToReturnDto>> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    {
        try
        {
            if (createOrderDTO.PaymentMethod.ToLower() == "card")
            {
                return new ApiResponse<OrderToReturnDto>(400, "Use ConfirmPaymentAndCreateOrderAsync for Visa payments.");
            }

            var cart = await _unitOfWork.CartRepo.GetByUserId(createOrderDTO.UserId);
            if (cart == null || !cart.CartDetails.Any())
            {
                return new ApiResponse<OrderToReturnDto>(404, "Cart is empty or not found.");
            }

            var payment = new Payment
            {
                Method = createOrderDTO.PaymentMethod,
                PaymentDate = DateTime.UtcNow,
                PaymentStatus = "pending"
            };

            _unitOfWork.PaymentRepository.Add(payment);
            await _unitOfWork.CompleteAsync();

            var order = await CreateOrderInternalAsync(cart, createOrderDTO, payment);
            return new ApiResponse<OrderToReturnDto>(200, "Order created successfully", order);
        }
        catch (Exception ex)
        {
            return new ApiResponse<OrderToReturnDto>(500, $"Something went wrong: {ex.Message}");
        }
    }

    public async Task<OrderToReturnDto> CreateOrderInternalAsync(Cart cart, CreateOrderDTO dto, Payment payment)
    {
        List<OrderDetail> orderDetails = new List<OrderDetail>();
        decimal orderSubTotal = 0;

        foreach (var cartDetail in cart.CartDetails)
        {
            var inventories = await _unitOfWork.StockProductInventoryRepo
                .GetAvailableInventoriesForProduct(cartDetail.ProductId);

            var sortedInventories = inventories
                .Where(i => i.StockQuantity > 0)
                .OrderBy(i => Helper.CalculateDistance(dto.Lat, dto.Long,i.Inventory.Lat,i.Inventory.Long))
                .ToList();

            int orderedQty = cartDetail.Quantity;
            decimal totalPrice = 0;
            int totalTaken = 0;

            var tempInventoryUpdates = new List<(StockProductInventory inventory, int takeQty)>();

            foreach (var inventory in sortedInventories)
            {
                if (orderedQty <= 0) break;

                int takeQty = Math.Min(inventory.StockQuantity, orderedQty);
                tempInventoryUpdates.Add((inventory, takeQty));
                totalPrice += takeQty * inventory.StockUnitPrice;
                totalTaken += takeQty;
                orderedQty -= takeQty;
            }

            
            if (totalTaken < cartDetail.Quantity)
                throw new Exception($"Not enough inventory for product {cartDetail.ProductId}");

            
            foreach (var (inventory, takeQty) in tempInventoryUpdates)
            {
                inventory.StockQuantity -= takeQty;

                if (inventory.StockQuantity <= 0)
                    inventory.IsDeleted = true;

                _unitOfWork.StockProductInventoryRepo.Update(inventory);
                await _unitOfWork.CompleteAsync();

                await _unitOfWork.StockProductInventoryRepo.CheckStockAndNotify(inventory.ProductId);
            }


            if (totalTaken == 0)
                throw new Exception($"No stock taken for product {cartDetail.ProductId}, unable to calculate price.");

            decimal avgPrice = totalPrice / totalTaken;
            orderSubTotal += avgPrice * totalTaken;

            orderDetails.Add(new OrderDetail
            {
                ProductId = cartDetail.ProductId,
                SellQuantity = totalTaken,
                SellPrice = avgPrice
            });
        }

        decimal tax = orderSubTotal * 0.14m;
        decimal shipping = 10;
        decimal total = orderSubTotal + tax + shipping;

        var order = new OrderHeader
        {
            CartId = cart.CartId,
            OrderDate = DateTime.UtcNow,
            OrderStatus = "shipped",
            OrderSubtotal = orderSubTotal,
            OrderTax = tax,
            OrderShippingCost = shipping,
            OrderTotalAmount = total,
            PaymentId = payment.PaymentId,
            PaymentIntentId = payment.PaymentIntentId
        };

        _unitOfWork.OrderRepo.Add(order);
        await _unitOfWork.CompleteAsync();

        foreach (var detail in orderDetails)
        {
            detail.OrderHeaderId = order.OrderHeaderId;
            _unitOfWork.OrderDetailRepo.Add(detail);
        }

        cart.IsDeleted = true;
        _unitOfWork.CartRepo.Update(cart);

        await _unitOfWork.UserRepo.UpdateUserAddress(dto.UserId, dto.Address);
        await _unitOfWork.UserRepo.UpdateUserPhoneNumber(dto.UserId, dto.PhoneNumber);

        await _unitOfWork.CompleteAsync();

        return _mapper.Map<OrderToReturnDto>(order);
    }
    #endregion


    #region  Abdelazez FInish Order 



    //public async Task<OrderToReturnDto> CreateOrderAsync(CreateOrderDTO createOrderDTO)
    //{
    //    // 1. Get cart
    //    var cart = await _unitOfWork.CartRepo.GetByUserId(createOrderDTO.UserId);
    //    if (cart == null || !cart.CartDetails.Any())
    //        throw new Exception("Cart is empty or not found.");

    //    // 2. Prepare order details and subtotal
    //    List<OrderDetail> orderDetails = new List<OrderDetail>();
    //    decimal orderSubTotal = 0;

    //    foreach (var cartDetail in cart.CartDetails)
    //    {
    //        if (cartDetail.Product == null)
    //            throw new Exception($"Product with ID {cartDetail.ProductId} not found in cart.");


    //        #region Calc Price And Inventory

    //        var inventories = await _unitOfWork.StockProductInventoryRepo.GetAvailableInventoriesForProduct(cartDetail.ProductId);

    //        var sortedInventories = inventories
    //            .Where(i => i.StockQuantity > 0)
    //            .OrderBy(i => Helper.CalculateDistance(
    //                createOrderDTO.Lat,
    //                createOrderDTO.Long,
    //                i.Inventory.Lat,
    //                i.Inventory.Long))
    //            .ToList();

    //        int remainingQty = cartDetail.Quantity;
    //        decimal totalPrice = 0;
    //        int totalTaken = 0;

    //        foreach (var inventory in sortedInventories)
    //        {
    //            if (remainingQty <= 0) break;

    //            int takeQty = Math.Min(inventory.StockQuantity, remainingQty);
    //            inventory.StockQuantity -= takeQty;
    //            totalPrice += takeQty * inventory.StockUnitPrice;
    //            totalTaken += takeQty;
    //            remainingQty -= takeQty;

    //            if (inventory.StockQuantity <= 0)
    //                inventory.IsDeleted = true;

    //            _unitOfWork.StockProductInventoryRepo.Update(inventory);
    //        }

    //        if (totalTaken < cartDetail.Quantity)
    //            throw new Exception($"Not enough inventory for product {cartDetail.ProductId}");

    //        decimal avgPrice = totalPrice / totalTaken;
    //        orderSubTotal += avgPrice * totalTaken;

    //        orderDetails.Add(new OrderDetail
    //        {
    //            ProductId = cartDetail.ProductId,
    //            SellQuantity = totalTaken,
    //            SellPrice = avgPrice
    //        });
    //    }

    //    // 3. Calculate tax, shipping, total
    //    decimal tax = orderSubTotal * 0.14m;
    //    decimal shippingCost = 10;
    //    decimal totalAmount = orderSubTotal + tax + shippingCost;

    //    #endregion


    //    // 4. Create and save Payment
    //    var mappedCartDto = await _paymentServices.CreatePaymentIntent(cart.UserId,totalAmount);

    //    var newPayment = new Payment
    //    {
    //        Method = createOrderDTO.PaymentMethod,
    //        PaymentDate = DateTime.UtcNow,
    //        PaymentStatus = "pending",
    //        PaymentIntentId = mappedCartDto.PaymentIntentId,
    //    };

    //    _unitOfWork.PaymentRepository.Add(newPayment);
    //    await _unitOfWork.CompleteAsync(); // Save to get PaymentId

    //    // 5. Create and save OrderHeader
    //    var orderHeader = new OrderHeader
    //    {
    //        PaymentId = newPayment.PaymentId,
    //        PaymentIntentId= mappedCartDto.PaymentIntentId,
    //        CartId = cart.CartId,
    //        OrderDate = DateTime.UtcNow,
    //        OrderStatus = "shipped",
    //        OrderSubtotal = orderSubTotal,
    //        OrderTax = tax,
    //        OrderShippingCost = shippingCost,
    //        OrderTotalAmount = totalAmount,
    //    };

    //    var existingOrder = await _unitOfWork.OrderRepo.GetById(cart.CartId);
    //    if (existingOrder != null)
    //        throw new Exception("An order already exists for this cart.");

    //    _unitOfWork.OrderRepo.Add(orderHeader);
    //    await _unitOfWork.CompleteAsync(); // Save to get OrderHeaderId


    //    // Update payment and order with intent details
    //    newPayment.PaymentIntentId = mappedCartDto.PaymentIntentId;
    //    orderHeader.PaymentIntentId = mappedCartDto.PaymentIntentId;
    //    await _unitOfWork.CompleteAsync();

    //    // 7. Add OrderDetails
    //    foreach (var detail in orderDetails)
    //    {
    //        detail.OrderHeaderId = orderHeader.OrderHeaderId;
    //        _unitOfWork.OrderDetailRepo.Add(detail);
    //    }

    //    cart.IsDeleted = true;

    //    await _unitOfWork.CompleteAsync();


    //    // 9. Update user info
    //    await _unitOfWork.UserRepo.UpdateUserAddress(createOrderDTO.UserId, createOrderDTO.Address);
    //    await _unitOfWork.UserRepo.UpdateUserPhoneNumber(createOrderDTO.UserId, createOrderDTO.PhoneNumber);
    //    await _unitOfWork.CompleteAsync();

    //    // 10. Return the created order
    //    var orderToReturn = _mapper.Map<OrderToReturnDto>(orderHeader);
    //    // 8. Delete cart
    //    //_unitOfWork.CartRepo.Update(cart);

    //    return orderToReturn;
    //}


    //  get Order by Id
    public async Task<orderDTO> GetOrderByIdAsync(int orderId)
    {
        if (orderId <= 0)
            throw new Exception("Order number Encorrect");
        var order = await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(orderId);
        if (order is null)
            throw new Exception("Order Not Found");
        return _mapper.Map<orderDTO>(order);


    }



    public async Task<List<orderDTO>> GetOrdersByUserIdAsync(string userId)
    {
        var orders = await _unitOfWork.OrderRepo.GetOrdersByUserIdAsync(userId);

        var result = _mapper.Map<List<orderDTO>>(orders);

        return result;
    }


    public async Task<bool> DeleteOrderAsync(int orderId)
    {
        var order = await _unitOfWork.OrderRepo.GetOrderByIdWithDetails(orderId);

        if (order is null) return false;
       
            //foreach (var item in order.OrderDetails)
            //{
            //    //item.IsDeleted = true;
            //    _unitOfWork.OrderDetailRepo.Update(item);
            //}
            //order.IsDeleted = true;
            order.OrderStatus = "cancelled";
        
        _unitOfWork.OrderRepo.Update(order);

        var inventoryReturned = await _inventoryService.ReturnInventoryQuantityAfterOrderDelete(orderId);

        if (!inventoryReturned) return false;

        await _unitOfWork.CompleteAsync();
        return true;
    }




    #endregion



}

