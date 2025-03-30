using AutoMapper;
using Blink_API.DTOs.DiscountDTO;

namespace Blink_API.Services.DiscountServices
{
    public class DiscountService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public DiscountService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<ICollection<DiscountDetailsDTO>> GetRunningDiscounts()
        {
            var discounts = await unitOfWork.DiscountRepo.GetRunningDiscounts();
            var mappedDiscount = mapper.Map<ICollection<DiscountDetailsDTO>>(discounts);
            return mappedDiscount;
        }
    }
}
