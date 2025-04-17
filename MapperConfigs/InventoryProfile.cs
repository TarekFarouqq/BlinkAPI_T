using AutoMapper;
using Blink_API.DTOs.InventoryDTOS;
using Blink_API.Models;

namespace Blink_API.MapperConfigs
{
    public class InventoryProfile : Profile
    {
        public InventoryProfile()
        {
            CreateMap<Inventory, ReadInventoryDTO>().ForMember(dest => dest.BranchName, option => option.MapFrom(src => src.Branch.BranchName));
            // ------------------------------------------------------------------------
            CreateMap<AddInventoryDTO, Inventory>();
            // ------------------------------------------------------------------------
        }
    }
}
