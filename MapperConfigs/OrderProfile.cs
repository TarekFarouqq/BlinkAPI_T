using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.OrdersDTO;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;
using Blink_API.Services.PaymentServices;

namespace Blink_API.MapperConfigs
{
    public class OrderProfile : Profile
    {
        public OrderProfile()
        {
            CreateMap<CreateOrderDTO, OrderHeader>()
                .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
                .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => new Payment
                {Method = src.PaymentMethod,PaymentStatus = "pending",PaymentDate = DateTime.UtcNow}));
            // ------------------------------------------------------------------------
            CreateMap<CreateOrderDTO, OrderHeader>()
               .ForMember(dest => dest.OrderDate, opt => opt.MapFrom(src => DateTime.UtcNow))
               .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => new Payment
               {
                   Method = src.PaymentMethod,
                   PaymentStatus = "pending",
                   PaymentDate = DateTime.UtcNow
               }));
            // ------------------------------------------------------------------------
            CreateMap<CartPaymentDTO, Cart>().ForMember(dest => dest.CartDetails, opt => opt.MapFrom(src => src.Items));
            // ------------------------------------------------------------------------
             CreateMap<OrderHeader, OrderToReturnDto>()
                 .ForMember(dest => dest.Payment, opt => opt.MapFrom(src => src.Payment))
                 .ForMember(dest => dest.OrderDetails, opt => opt.MapFrom(src => src.OrderDetails));
            // ------------------------------------------------------------------------
            CreateMap<OrderDetail, OrderDetailDto>()
                .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.product.ProductId))
                .ForMember(dest => dest.SellQuantity, opt => opt.MapFrom(src => src.SellQuantity))
                .ForMember(dest => dest.SellPrice, opt => opt.MapFrom(src => src.SellPrice));
            // ------------------------------------------------------------------------
            CreateMap<Payment, PaymentDto>()
                .ForMember(dest => dest.Method, opt => opt.MapFrom(src => src.Method))
                .ForMember(dest => dest.PaymentStatus, opt => opt.MapFrom(src => src.PaymentStatus))
                .ForMember(dest => dest.PaymentDate, opt => opt.MapFrom(src => src.PaymentDate));
            // ------------------------------------------------------------------------
            CreateMap<CartPaymentDTO, CustomerCart>();
            // ------------------------------------------------------------------------
            CreateMap<CartPaymentDTO, Cart>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<CartPaymentDTO, ReadCartDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<OrderHeader, orderDTO>()
               .ForMember(dest => dest.OrderId, opt => opt.MapFrom(src => src.OrderHeaderId))
               .ForMember(dest => dest.Total, opt => opt.MapFrom(src => src.OrderTotalAmount))
               .ForMember(dest => dest.Shipping, opt => opt.MapFrom(src => src.OrderShippingCost))
               .ForMember(dest => dest.Tax, opt => opt.MapFrom(src => src.OrderTax))
               .ForMember(dest => dest.Subtotal, opt => opt.MapFrom(src => src.OrderTotalAmount - src.OrderShippingCost - src.OrderTax))
               .ForMember(dest => dest.PaymentIntentId, opt => opt.MapFrom(src => src.Payment.PaymentIntentId))
               .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.OrderDetails));

            CreateMap<OrderDetail, ConfirmedOrderItemDTO>()
                .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.product.ProductName))
                .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src =>
                    src.product.ProductImages.Any() ? src.product.ProductImages.First().ProductImagePath : string.Empty
                )).ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.SellQuantity))
                .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.SellPrice));

            // ------------------------------------------------------------------------
        }
    }
}
