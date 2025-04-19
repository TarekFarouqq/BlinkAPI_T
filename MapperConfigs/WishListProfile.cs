using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.PaymentCart;
using Blink_API.DTOs.WishListDTOs;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class WishListProfile : Profile
    {
        public WishListProfile()
        {
            CreateMap<WishList, ReadWishListDTO>()
                .ForMember(dest => dest.UserId, option => option.MapFrom(src => src.UserId))
                .ForMember(dest => dest.WishListId, option => option.MapFrom(src => src.WishlistId))
                .ForMember(dest => dest.WishLishDetails, option => option.MapFrom(src => src.WishListDetails.Select(r => new WishLishDetailsDTO
                {
                    ProductId = r.Product.ProductId,
                    ProductName = r.Product.ProductName,
                    ProductImageUrl = r.Product.ProductImages.FirstOrDefault().ProductImagePath
                }))).ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<WishListDetail, WishLishDetailsDTO>()
              .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
              .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.ProductName))
              .ForMember(dest => dest.ProductImageUrl, opt => opt.MapFrom(src => src.Product.ProductImages.FirstOrDefault().ProductImagePath));
            // ------------------------------------------------------------------------

        }
    }
}

