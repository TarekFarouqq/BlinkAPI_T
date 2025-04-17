using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.PaymentCart;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class CartProfile : Profile
    {
        public CartProfile()
        {
            CreateMap<Cart, ReadCartDTO>()
                .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.UserId))
                .ForMember(dest => dest.CartId, option => option.MapFrom(src => src.CartId))
                .ForMember(dest => dest.CartDetails, option => option.MapFrom(src => src.CartDetails.Select(r => new CartDetailsDTO
                {
                    ProductId = r.Product.ProductId,
                    ProductName = r.Product.ProductName,
                    ProductImageUrl = r.Product.ProductImages.FirstOrDefault().ProductImagePath,
                    ProductUnitPrice = r.Product.StockProductInventories.Any() == true ? r.Product.StockProductInventories.Average(p => p.StockUnitPrice) : 0,
                    Quantity = r.Quantity
                }))).ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<CartDetail, CartDetailsDTO>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
              .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ProductImagePath))
              .ForMember(dest => dest.ProductUnitPrice, opt => opt.MapFrom(src =>
                  src.Product.ProductDiscounts
                      .Where(d => !d.IsDeleted)
                      .OrderByDescending(d => d.DiscountAmount)
                      .Select(d => d.DiscountAmount)
                      .FirstOrDefault()))
              .ForMember(dest => dest.Quantity, opt => opt.MapFrom(src => src.Quantity));
            // ------------------------------------------------------------------------
            CreateMap<ReadCartDTO, CartPaymentDTO>().ReverseMap();
            // ------------------------------------------------------------------------
        }
    }
}
