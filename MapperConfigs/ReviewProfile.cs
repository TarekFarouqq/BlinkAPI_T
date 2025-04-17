using AutoMapper;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class ReviewProfile : Profile
    {
        public ReviewProfile()
        {
            CreateMap<UserReviewCommentDTO, Review>()
            .ForMember(dest => dest.UserId, opt => opt.MapFrom(src => src.UserId))
            .ForMember(dest => dest.ProductId, opt => opt.MapFrom(src => src.ProductId))
            .ForMember(dest => dest.Rate, opt => opt.MapFrom(src => src.ReviewRate))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ReviewComments, opt => opt.MapFrom(src => new List<ReviewComment>
            {
                new ReviewComment
                {
                    Content = src.Comment,
                    IsDeleted = false
                }
            }))
            .ForMember(dest => dest.IsDeleted, opt => opt.MapFrom(src => false)).ReverseMap();

        }
    }
}
