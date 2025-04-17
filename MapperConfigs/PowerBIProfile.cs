using AutoMapper;
using Blink_API.DTOs.BiDataDtos;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Blink_API.Services.PaymentServices;
using Microsoft.AspNetCore.Identity;

namespace Blink_API.MapperConfigs
{
    public class PowerBIProfile : Profile
    {
        public PowerBIProfile()
        {
            // 1- stock_fact :
            CreateMap<StockProductInventory, stock_factDto>()
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.Product.ProductId))
                .ForMember(dest => dest.InventoryId, option => option.MapFrom(src => src.Inventory.InventoryId))
                .ForMember(dest => dest.StockUnitPrice, option => option.MapFrom(src => src.StockUnitPrice))
                .ForMember(dest => dest.StockQuantity, option => option.MapFrom(src => src.StockQuantity))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // 2- review diminsiion :
            CreateMap<Review, Review_DimensionDto>()
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.ReviewComments, opt => opt.MapFrom(src => src.ReviewComments.Select(c => c.Content).ToList()))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // 3- Payment
            CreateMap<CartPaymentDTO, CustomerCart>();
            CreateMap<CartPaymentDTO, Cart>().ReverseMap();
            CreateMap<CartPaymentDTO, ReadCartDTO>().ReverseMap();
            // payment dimension :
            CreateMap<Payment, Payment_DimensionDto>()
                .ForMember(dest => dest.Method, option => option.MapFrom(src => src.Method))
                .ForMember(dest => dest.PaymentDate, option => option.MapFrom(src => src.PaymentDate))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // user role :
            CreateMap<IdentityUserRole<string>, UserRoles_DimensionDto>()
                .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.UserId))
                .ForMember(dest => dest.RoleId, option => option.MapFrom(src => src.RoleId))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // role dimension :
            CreateMap<IdentityRole, Role_DiminsionDto>()
                .ForMember(dest => dest.RoleId, option => option.MapFrom(src => src.Id))
                .ForMember(dest => dest.RoleName, option => option.MapFrom(src => src.Name))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ApplicationUser, User_DimensionDto>()
                .ForMember(dest => dest.User_ID, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.User_Name, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.First_Name, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.Last_Name, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.PasswordHash))
                .ForMember(dest => dest.Last_Modification, opt => opt.MapFrom(src => src.LastModification))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.E_mail, opt => opt.MapFrom(src => src.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Created_in, opt => opt.MapFrom(src => src.CreatedIn))
                .ForMember(dest => dest.User_Granted, opt => opt.MapFrom(src => src.UserGranted))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // inventory transaction :
            CreateMap<InventoryTransactionHeader, Inventory_Transaction_Dto>()
               .ForMember(dest => dest.InventoryTransactionHeaderId, option => option.MapFrom(src => src.InventoryTransactionHeaderId))
               .ForMember(dest => dest.InventoryId, option => option.MapFrom(src => src.Inventories.FirstOrDefault().InventoryId))
               .ReverseMap();
            // ------------------------------------------------------------------------
            // cart diminsion :
            CreateMap<Cart, cart_DiminsionDto>()
                .ForMember(dest => dest.CreationDate, option => option.MapFrom(src => src.CartDetails.FirstOrDefault().CreationDate))
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.CartDetails.FirstOrDefault().ProductId))
                .ForMember(dest => dest.Quantity, option => option.MapFrom(src => src.CartDetails.FirstOrDefault().Quantity))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // order_fact :
            CreateMap<OrderDetail, order_FactDto>()
                .ForMember(dest => dest.OrderId, option => option.MapFrom(src => src.OrderHeader.OrderHeaderId))
                .ForMember(dest => dest.PaymentId, option => option.MapFrom(src => src.OrderHeader.PaymentId))
                .ForMember(dest => dest.CartId, option => option.MapFrom(src => src.OrderHeader.CartId))
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.ProductId))
                .ForMember(dest => dest.OrderDetailId, option => option.MapFrom(src => src.OrderDetailId))
                .ForMember(dest => dest.OrderDate, option => option.MapFrom(src => src.OrderHeader.OrderDate))
                .ForMember(dest => dest.Subtotal, option => option.MapFrom(src => src.OrderHeader.OrderSubtotal))
                .ForMember(dest => dest.Tax, option => option.MapFrom(src => src.OrderHeader.OrderTax))
                .ForMember(dest => dest.ShippingCost, option => option.MapFrom(src => src.OrderHeader.OrderShippingCost))
                .ForMember(dest => dest.TotalAmount, option => option.MapFrom(src => src.OrderHeader.OrderTotalAmount))
                .ForMember(dest => dest.OrderStatus, option => option.MapFrom(src => src.OrderHeader.OrderStatus))
                .ForMember(dest => dest.Quantity, option => option.MapFrom(src => src.SellQuantity))
                .ForMember(dest => dest.SellPrice, option => option.MapFrom(src => src.SellPrice))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // branch inventory :
            CreateMap<Branch, Branch_inventoryDto>()
                .ForMember(dest => dest.InventoryId, option => option.MapFrom(src => src.Inventories.FirstOrDefault().InventoryId))
                .ForMember(dest => dest.InventoryName, option => option.MapFrom(src => src.Inventories.FirstOrDefault().InventoryName))
                .ForMember(dest => dest.InventoryAddress, option => option.MapFrom(src => src.Inventories.FirstOrDefault().InventoryAddress))
                .ForMember(dest => dest.BranchPhone, option => option.MapFrom(src => src.Phone))
                .ForMember(dest => dest.BranchAddress, option => option.MapFrom(src => src.BranchAddress))
                .ForMember(dest => dest.InventoryPhone, option => option.MapFrom(src => src.Inventories.FirstOrDefault().Phone))
                .ReverseMap();
            // ------------------------------------------------------------------------
            // inventory transaction fact :
            CreateMap<TransactionDetail, InventoryTransaction_FactDto>()
                .ForMember(dest => dest.TransactionDate, option => option.MapFrom(src => src.InventoryTransactionHeader.InventoryTransactionDate))
                .ForMember(dest => dest.TransactionType, option => option.MapFrom(src => src.InventoryTransactionHeader.InventoryTransactionType))
                .ForMember(dest => dest.InventoryTransactionHeaderId, option => option.MapFrom(src => src.InventoryTransactionHeaderId))
                .ForMember(dest => dest.SrcInventoryId, option => option.MapFrom(src => src.SrcInventory.InventoryId))
                .ForMember(dest => dest.DistInventoryId, option => option.MapFrom(src => src.DistInventory.InventoryId))
                .ForMember(dest => dest.InventoryTransactionHeaderId, option => option.MapFrom(src => src.InventoryTransactionHeader.InventoryTransactionHeaderId))
                .ReverseMap();
            // ------------------------------------------------------------------------
        }
    }
}
