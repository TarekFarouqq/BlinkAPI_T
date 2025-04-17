using AutoMapper;
using Blink_API.DTOs.BrandDtos;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class BrandProfile : Profile
    {
        public BrandProfile()
        {
            CreateMap<Brand, BrandDTO>().ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<insertBrandDTO, Brand>()
                .ForMember(dest => dest.BrandImage, option => option.Ignore()).ReverseMap();
            // ------------------------------------------------------------------------
        }
    }
}
