using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.DTOs.CategoryDTOs;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class CategoryProfile : Profile
    {
        public CategoryProfile()
        {
            CreateMap<Category, ParentCategoryDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Category, ChildCategoryDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<Category, ReadCategoryDTO>()
               .ForMember(dest => dest.SubCategories, opt => opt.MapFrom(src =>
                   src.SubCategories.Where(c => !c.IsDeleted)));
            // ------------------------------------------------------------------------
            CreateMap<Category, ReadChildCategoryDTO>();
            // ------------------------------------------------------------------------
            CreateMap<InsertChildCategoryDTO, Category>()
           .ForMember(dest => dest.CategoryImage, opt => opt.Ignore())
           .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
           .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
           .ForMember(dest => dest.ParentCategoryId, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<InsertCategoryDTO, Category>()
                .ForMember(dest => dest.CategoryImage, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategoryId, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<UpdateParentCategoryDTO, Category>()
            .ForMember(dest => dest.CategoryImage, opt => opt.Ignore())
            .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
            .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<UpdateChildCategoryDTO, Category>()
                .ForMember(dest => dest.CategoryImage, opt => opt.Ignore())
                .ForMember(dest => dest.SubCategories, opt => opt.Ignore())
                .ForMember(dest => dest.ParentCategory, opt => opt.Ignore());
            // ------------------------------------------------------------------------
            CreateMap<CreateCategoryDTO, Category>();
            // ------------------------------------------------------------------------
        }
    }
}
