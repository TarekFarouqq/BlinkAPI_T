using AutoMapper;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.DTOs.UsersDtos;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<RegisterDto, ApplicationUser>()
                // .ForSourceMember(src => src.Role, opt => opt.DoNotValidate())
               //  .ForMember(dest => dest.Role, opt => opt.Ignore())
               .ForMember(dest => dest.FirstName, option => option.MapFrom(src => src.FName))
               .ForMember(dest => dest.LastName, option => option.MapFrom(src => src.LName))
               .ForMember(dest => dest.Email, option => option.MapFrom(src => src.Email))
               .ForMember(dest => dest.PhoneNumber, option => option.MapFrom(src => src.PhoneNumber))
               .ForMember(dest => dest.Address, option => option.MapFrom(src => src.Address))
               .ForMember(dest => dest.UserName, option => option.MapFrom(src => src.UserName))
               .ForMember(dest => dest.LastModification, option => option.MapFrom(src => DateTime.Now))
               .ReverseMap();
            // ------------------------------------------------------------------------
       //    CreateMap<ApplicationUser, UserDto>().ForMember(dest => dest.Role, opt => opt.Ignore());
            // ------------------------------------------------------------------------
       //    CreateMap<ApplicationUser, UserDto>().ForMember(dest => dest.Role, opt => opt.Ignore()).ReverseMap();
            // ------------------------------------------------------------------------
       //     CreateMap<ApplicationUser, AddUserDto>().ForMember(dest => dest.Role, opt => opt.Ignore()).ReverseMap();
            // ------------------------------------------------------------------------
            CreateMap<ApplicationUser, UserDto>();
            CreateMap<ApplicationUser, UserDto>().ReverseMap();

            CreateMap<AddUserDto, ApplicationUser>();



        }
    }
}
