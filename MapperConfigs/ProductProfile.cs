using AutoMapper;
using Blink_API.DTOs.BiDataDtos;
using Blink_API.DTOs.DiscountDTO;
using Blink_API.DTOs.Product;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class ProductProfile:Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductDiscountsDTO>()
               .ForMember(dest => dest.SupplierName, option => option.MapFrom(src => $"{src.User.FirstName} {src.User.LastName}"))
               .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.BrandName))
               .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
               .ForMember(dest => dest.ProductImages, option => option.MapFrom(src => src.ProductImages.Select(img => img.ProductImagePath).ToList()))
               .ForMember(dest => dest.AverageRate, option => option.MapFrom(src => src.Reviews.Any() == true ? src.Reviews.Average(r => r.Rate) : 0))
               .ForMember(dest => dest.ProductReviews, option => option.MapFrom(src => src.Reviews))
               .ForMember(dest => dest.CountOfRates, option => option.MapFrom(src => src.Reviews.Select(r => r.ReviewId).Count()))
               .ForMember(dest => dest.ProductPrice, option => option.MapFrom(src => src.StockProductInventories.Any() == true ? src.StockProductInventories.Average(p => p.StockUnitPrice) : 0))
               .ForMember(dest => dest.StockQuantity, option => option.MapFrom(src => src.StockProductInventories.Any() == true ? src.StockProductInventories.Sum(s => s.StockQuantity) : 0))
               .ForMember(dest => dest.DiscountPercentage, option => option.MapFrom(src => src.ProductDiscounts
               .Where(pd => !pd.IsDeleted && !pd.Discount.IsDeleted && pd.Discount.DiscountFromDate <= DateTime.UtcNow && pd.Discount.DiscountEndDate >= DateTime.UtcNow)
               .Sum(pd => pd.Discount.DiscountPercentage)))
               .ForMember(dest => dest.DiscountAmount, option => option.MapFrom(src => src.ProductDiscounts.Where(pd => !pd.IsDeleted && !pd.Discount.IsDeleted && pd.Discount.DiscountFromDate <= DateTime.UtcNow && pd.Discount.DiscountEndDate >= DateTime.UtcNow).Sum(pd => pd.DiscountAmount)))
               .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Review, ReviewCommentDTO>()
                .ForMember(dest => dest.Username, option => option.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.Rate, option => option.MapFrom(src => src.Rate))
                .ForMember(dest => dest.ReviewComment, option => option.MapFrom(src => src.ReviewComments.Select(c => c.Content)));
            // ------------------------------------------------------------------------
            CreateMap<Product, InsertProductDTO>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore())
                .ForMember(dest => dest.OldImages, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<InsertProductDTO, Product>()
                .ForMember(dest => dest.ProductImages, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<Product, InsertProductDTO>()
                .ForMember(dest => dest.ProductStocks, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<UpdateProductDTO, Product>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertProductImagesDTO, ProductImage>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertFilterAttributeDTO, FilterAttributes>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertDefaultAttributesDTO, DefaultAttributes>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ReadFilterAttributesDTO, FilterAttributes>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ReadDefaultAttributesDTO, DefaultAttributes>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertProductAttributeDTO, ProductAttributes>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertProductStockDTO, StockProductInventory>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ProductDiscount, Product_DiscountDto>()
               .ForMember(dest => dest.DiscountId, option => option.MapFrom(src => src.DiscountId))
               .ForMember(dest => dest.DiscountAmount, option => option.MapFrom(src => src.DiscountAmount))
               .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Discount, Discount_DimensionDto>()
                .ForMember(dest => dest.DiscountPercentage, option => option.MapFrom(src => src.DiscountPercentage))
                .ForMember(dest => dest.DiscountFromDate, option => option.MapFrom(src => src.DiscountFromDate))
                .ForMember(dest => dest.DiscountEndDate, option => option.MapFrom(src => src.DiscountEndDate))
                .ForMember(dest => dest.DiscountAmount, option => option.MapFrom(src => src.ProductDiscounts.FirstOrDefault().DiscountAmount))
                .ForMember(dest => dest.ProductId, option => option.MapFrom(src => src.ProductDiscounts.FirstOrDefault().ProductId))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Product, Product_DiminsionDto>()
                .ForMember(dest => dest.ParentCategoryId, option => option.MapFrom(src => src.Category.ParentCategoryId))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.CategoryDescription, option => option.MapFrom(src => src.Category.CategoryDescription))
                .ForMember(dest => dest.CategoryImage, option => option.MapFrom(src => src.Category.CategoryImage))
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.BrandId, option => option.MapFrom(src => src.Brand.BrandId))
                .ForMember(dest => dest.BrandImage, option => option.MapFrom(src => src.Brand.BrandImage))
                .ForMember(dest => dest.BrandDescription, option => option.MapFrom(src => src.Brand.BrandDescription))
                .ForMember(dest => dest.BrandWebSiteURL, option => option.MapFrom(src => src.Brand.BrandWebSiteURL))
                .ForMember(dest => dest.ProductImagePaths, opt => opt.MapFrom(src =>
                    src.ProductImages.Select(pi => pi.ProductImagePath).ToList()))
                .ForMember(dest => dest.ProductImagePaths, option => option.MapFrom(src => src.ProductImages.FirstOrDefault().ProductImagePath))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<InsertReviewSuppliedProductDTO, ReviewSuppliedProduct>();
            // ------------------------------------------------------------------------
            CreateMap<ReviewSuppliedProductImages, ReadReviewSuppliedProductImagesDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ReviewSuppliedProduct, ReadReviewSuppliedProductDTO>()
                .ForMember(dest => dest.BrandName, option => option.MapFrom(src => src.Brand.BrandName))
                .ForMember(dest => dest.InventoryName, option => option.MapFrom(src => src.Inventory.InventoryName))
                .ForMember(dest => dest.CategoryName, option => option.MapFrom(src => src.Category.CategoryName))
                .ForMember(dest => dest.SupplierName, option => option.MapFrom(src => $"{src.Supplier.FirstName} {src.Supplier.LastName}"))
                .ForMember(dest => dest.ProductImages, option => option.MapFrom(src => src.ReviewSuppliedProductImages))
                .ForMember(dest => dest.ProductQuantity, option => option.MapFrom(src => src.ProductQuantity))
                .ForMember(dest => dest.ProductPrice, option => option.MapFrom(src => src.ProductPrice))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ReviewSuppliedProductImages, ReadReviewSuppliedProductImagesDTO>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.ImagePath));
            // ------------------------------------------------------------------------
            CreateMap<Product, ReadReviewSuppliedProductDTO>()
                .ForMember(dest => dest.ProductName, option => option.MapFrom(src => src.ProductName))
                .ForMember(dest => dest.ProductDescription, option => option.MapFrom(src => src.ProductDescription))
                .ForMember(dest => dest.SupplierId, option => option.MapFrom(src => src.SupplierId))
                .ForMember(dest => dest.BrandId, option => option.MapFrom(src => src.BrandId))
                .ForMember(dest => dest.CategoryId, option => option.MapFrom(src => src.CategoryId))
                .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ReadReviewSuppliedProductImagesDTO, ReviewSuppliedProductImages>()
                .ForMember(dest => dest.ImagePath, opt => opt.MapFrom(src => src.ImageUrl))
                .ForMember(dest => dest.RequestId, opt => opt.MapFrom(src => src.RequestId));
            // ------------------------------------------------------------------------
            CreateMap<ReviewSuppliedProduct, Product>()
             .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.ProductName))
             .ForMember(dest => dest.ProductDescription, opt => opt.MapFrom(src => src.ProductDescription))
             .ForMember(dest => dest.SupplierId, opt => opt.MapFrom(src => src.SupplierId))
             .ForMember(dest => dest.BrandId, opt => opt.MapFrom(src => src.BrandId))
             .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
             .ForMember(dest => dest.ProductId, option => option.Ignore())
             .ForMember(dest => dest.ProductCreationDate, option => option.Ignore())
             .ForMember(dest => dest.ProductModificationDate, option => option.Ignore())
             .ForMember(dest => dest.ProductSupplyDate, option => option.Ignore())
             .ForMember(dest => dest.IsDeleted, option => option.Ignore())
             .ForMember(dest => dest.Category, option => option.Ignore())
             .ForMember(dest => dest.Brand, option => option.Ignore())
             .ForMember(dest => dest.User, option => option.Ignore())
             .ForMember(dest => dest.Reviews, option => option.Ignore())
             .ForMember(dest => dest.ProductImages, option => option.Ignore())
             .ForMember(dest => dest.TransactionProducts, option => option.Ignore())
             .ForMember(dest => dest.OrderDetails, option => option.Ignore())
             .ForMember(dest => dest.CartDetails, option => option.Ignore())
             .ForMember(dest => dest.WishListDetails, option => option.Ignore())
             .ForMember(dest => dest.ProductDiscounts, option => option.Ignore())
             .ForMember(dest => dest.StockProductInventories, option => option.Ignore())
             .ForMember(dest => dest.ProductAttributes, option => option.Ignore())
             .ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Discount, DiscountDetailsDTO>()
                .ForMember(dest => dest.DiscountProducts, option => option.MapFrom(src => src.ProductDiscounts.Select(dp => new DiscountProductDetailsDTO
                {
                    DiscountId = dp.DiscountId,
                    ProductId = dp.ProductId,
                    DiscountAmount = dp.DiscountAmount,
                    IsDeleted = dp.IsDeleted
                }))).ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Discount, ReadDiscountDetailsDTO>()
                .ForMember(dest=>dest.DiscountId,option=>option.MapFrom(src=>src.DiscountId))
                .ForMember(dest=>dest.DiscountPercentage, option=>option.MapFrom(src=>src.DiscountPercentage))
                .ForMember(dest=>dest.DiscountFromDate, option=>option.MapFrom(src=>src.DiscountFromDate))
                .ForMember(dest=>dest.DiscountEndDate, option=>option.MapFrom(src=>src.DiscountEndDate))
                .ForMember(dest=>dest.ReadProductsDiscountDTOs,option=>option.MapFrom(src=>src.ProductDiscounts))
                .ReverseMap();
            CreateMap<ProductDiscount, ReadProductsDiscountDTO>()
                .ForMember(dest=>dest.ProductName,option=>option.MapFrom(src=>src.Product.ProductName))
                .ForMember(dest=>dest.SubCategoryName,option=>option.MapFrom(src=>src.Product.Category.CategoryName))
                .ForMember(dest=>dest.ParentCategoryName,option=>option.MapFrom(src=>src.Product.Category.ParentCategory.CategoryName))
                .ForMember(dest=>dest.BrandName,option=>option.MapFrom(src=>src.Product.Brand.BrandName))
                .ForMember(dest => dest.ProductPrice, option => option.MapFrom(src => src.Product.StockProductInventories.Any() ? src.Product.StockProductInventories.Average(spi => spi.StockUnitPrice): 0))
                .ReverseMap();
            CreateMap<Discount, InsertDiscountDetailsDTO>().ReverseMap();
            CreateMap<ProductDiscount, InsertProductDiscountDetailsDTO>().ReverseMap();
            CreateMap<Discount, UpdateDiscountDetailsDTO>().ReverseMap();
            CreateMap<ProductDiscount, UpdateProductDiscountDetailsDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Product, ReadProductInStockDTO>()
                .ForMember(dest=>dest.StockQuantity,option=>option.MapFrom(src=>src.StockProductInventories.Sum(spi=>spi.StockQuantity)))
                .ReverseMap();
            // ------------------------------------------------------------------------
        }
    }
}
