using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.Models;
using System.Runtime.CompilerServices;

namespace Blink_API.MapperConfigs
{
    public class MapperConfig:Profile
    {
        public MapperConfig()
        {
            CreateMap<Category, ParentCategoryDTO>().ReverseMap();
            CreateMap<Category, ChildCategoryDTO>().ReverseMap();
        }
    }
}
