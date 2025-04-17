using AutoMapper;
using Blink_API.DTOs.BranchDto;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class BranchProfile : Profile
    {
        public BranchProfile()
        {
            CreateMap<Branch, ReadBranchDTO>();
            // ------------------------------------------------------------------------
            CreateMap<AddBranchDTO, Branch>();
            // ------------------------------------------------------------------------
        }
    }
}
