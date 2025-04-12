using System.Runtime.CompilerServices;
using AutoMapper;
using Blink_API.DTOs.Product;
using Blink_API.DTOs.Category;
using Blink_API.Models;
using Blink_API.DTOs.DiscountDTO;

using Blink_API.DTOs.CategoryDTOs;

using Blink_API.DTOs.ProductDTOs;
 
using Blink_API.DTOs.BrandDtos;
 
using Blink_API.DTOs.BranchDto;
using Blink_API.DTOs.InventoryDTOS;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.DTOs.CartDTOs;
using Blink_API.Services.PaymentServices;
using Blink_API.DTOs.PaymentCart;



namespace Blink_API.MapperConfigs
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, ParentCategoryDTO>().ReverseMap();
            ///////
            CreateMap<Category, ChildCategoryDTO>().ReverseMap();
            ///////
            CreateMap<Product, ProductDetailsDTO>()
                .ForMember(dest => dest.SupplierName, option => option.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ProductImages, option => option.MapFrom(src => src.ProductImages.Select(img => img.ProductImagePath).ToList()))
                .ForMember(dest => dest.AverageRate, option => option.MapFrom(src => src.Reviews.Any() == true ? src.Reviews.Average(r => r.Rate) : 0))
                .ForMember(dest => dest.ProductReviews, option => option.MapFrom(src => src.Reviews.Select(r => new ReviewCommentDTO
                {
                    Rate = r.Rate,
                    ReviewComment = r.ReviewComments.Select(rc => rc.Content).ToList()
                })))
                .ForMember(dest => dest.CountOfRates, option => option.MapFrom(src => src.Reviews.Select(r => r.ReviewId).Count()))
                .ForMember(dest => dest.ProductPrice, option => 
                option.MapFrom(src => src.StockProductInventories.Any() == true ? src.StockProductInventories.Average(p => p.StockUnitPrice):0))
                .ForMember(dest=>dest.StockQuantity,option=>
                option.MapFrom(src=>src.StockProductInventories.Any()==true? src.StockProductInventories.Sum(s=>s.StockQuantity) : 0 ))
                .ReverseMap();
            ///////
            CreateMap<Discount, DiscountDetailsDTO>()
                .ForMember(dest=>dest.DiscountProducts,option=>option.MapFrom(src=>src.ProductDiscounts.Select(dp=>new DiscountProductDetailsDTO
                {
                    DiscountId=dp.DiscountId,
                    ProductId = dp.ProductId,
                    DiscountAmount=dp.DiscountAmount,
                    IsDeleted=dp.IsDeleted
                })))
                .ReverseMap();
            ///////
            CreateMap<Cart,ReadCartDTO>()
                .ForMember(dest=>dest.UserId,option => option.MapFrom(src=> src.UserId))
                .ForMember(dest => dest.CartId, option => option.MapFrom(src => src.CartId))
                .ForMember(dest => dest.CartDetails, option => option.MapFrom(src => src.CartDetails.Select(r=> new CartDetailsDTO
                {
                    ProductId = r.Product.ProductId,
                    ProductName=r.Product.ProductName,
                    ProductImageUrl = r.Product.ProductImages.FirstOrDefault().ProductImagePath,
                    ProductUnitPrice = r.Product.StockProductInventories.Any() == true ? r.Product.StockProductInventories.Average(p => p.StockUnitPrice) : 0,
                    Quantity = r.Quantity
                }
            ))).ReverseMap();
            ///////


            CreateMap<CreateCategoryDTO, Category>();

            ///////
            CreateMap<Product, ProductDiscountsDTO>()
                .ForMember(dest => dest.SupplierName, option => option.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.ProductImages, option => option.MapFrom(src => src.ProductImages.Select(img => img.ProductImagePath).ToList()))
                .ForMember(dest => dest.AverageRate, option => option.MapFrom(src => src.Reviews.Any() == true ? src.Reviews.Average(r => r.Rate) : 0))
                 .ForMember(dest => dest.ProductReviews, option => option.MapFrom(src => src.Reviews.Select(r => new ReviewCommentDTO
                 {
                     Rate = r.Rate,
                     ReviewComment = r.ReviewComments.Select(rc => rc.Content).ToList()
                 })))
                .ForMember(dest => dest.CountOfRates, option => option.MapFrom(src => src.Reviews.Select(r => r.ReviewId).Count()))
                .ForMember(dest => dest.ProductPrice, option =>
                option.MapFrom(src => src.StockProductInventories.Any() == true ? src.StockProductInventories.Average(p => p.StockUnitPrice) : 0))
                .ForMember(dest => dest.StockQuantity, option =>
                option.MapFrom(src => src.StockProductInventories.Any() == true ? src.StockProductInventories.Sum(s => s.StockQuantity) : 0))
                .ForMember(dest => dest.DiscountPercentage, option => option.MapFrom(src => src.ProductDiscounts
                .Where(pd => !pd.IsDeleted && pd.Discount.DiscountFromDate <= DateTime.UtcNow && pd.Discount.DiscountEndDate >= DateTime.UtcNow)
                .Select(pd => pd.Discount.DiscountPercentage)
                .FirstOrDefault()))
                .ForMember(dest => dest.DiscountAmount, option => option.MapFrom(src => src.ProductDiscounts
                .Where(pd => !pd.IsDeleted && pd.Discount.DiscountFromDate <= DateTime.UtcNow && pd.Discount.DiscountEndDate >= DateTime.UtcNow)
                .Select(pd => pd.DiscountAmount)
                .FirstOrDefault())).ReverseMap();

            CreateMap<Product, InsertProductDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            CreateMap<InsertProductDTO, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());

            CreateMap<UpdateProductDTO, Product>().ReverseMap();

            CreateMap<InsertProductImagesDTO, ProductImage>().ReverseMap();

            //CreateMap<ProductImage, InsertProductImagesDTO>()
            //    .ForMember(dest=>dest.ProductId,option=>option.MapFrom(src => src.Product.ProductId))
            //    .ReverseMap();

            ////////////  
            CreateMap<Brand, BrandDTO>()
                .ReverseMap();

            CreateMap<insertBrandDTO,Brand >()
                .ReverseMap();
 
            ////////////
            CreateMap<Branch, ReadBranchDTO>();
            CreateMap<AddBranchDTO, Branch>();

            /////////////
            CreateMap<Inventory, ReadInventoryDTO>().ForMember(dest => dest.BranchName, option => option.MapFrom(src => src.Branch.BranchName));
            CreateMap<AddInventoryDTO, Inventory>();
            /////////////
            CreateMap<RegisterDto, ApplicationUser>()
                .ForMember(dest=>dest.FirstName,option=>option.MapFrom(src=>src.FName))
                .ForMember(dest=>dest.LastName,option=>option.MapFrom(src=>src.LName))
                .ForMember(dest=>dest.Email,option=>option.MapFrom(src=>src.Email))
                .ForMember(dest=>dest.PhoneNumber,option=>option.MapFrom(src=>src.PhoneNumber))
                .ForMember(dest=>dest.Address,option=>option.MapFrom(src=>src.Address))
                .ForMember(dest=>dest.UserName,option=>option.MapFrom(src=>src.UserName))
                .ForMember(dest=>dest.LastModification,option=>option.MapFrom(src=> DateTime.Now))
                .ReverseMap();


            //Payment
            CreateMap<CartPaymentDTO, CustomerCart>();
            CreateMap<CartPaymentDTO, Cart>().ReverseMap();
            CreateMap<CartPaymentDTO,ReadCartDTO>().ReverseMap();
            //CreateMap<BasketItemDto, BasketItem>();


        }
    }
}
