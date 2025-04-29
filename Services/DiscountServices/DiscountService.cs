using AutoMapper;
using Blink_API.DTOs.DiscountDTO;
using Blink_API.Models;

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
        public async Task<DiscountDetailsDTO> GetRunningDiscountById(int id)
        {
            var discount = await unitOfWork.DiscountRepo.GetRunningDiscountById(id);
            var mappedDiscount = mapper.Map<DiscountDetailsDTO>(discount);
            return mappedDiscount;
        }
        public async Task<ICollection<ReadDiscountDetailsDTO>> GetAllDiscounts()
        {
            var result = await unitOfWork.DiscountRepo.GetAllDiscounts();
            var mappedDiscount = mapper.Map<ICollection<ReadDiscountDetailsDTO>>(result);
            return mappedDiscount;
        }
        public async Task<ReadDiscountDetailsDTO> GetDiscountById(int id)
        {
            var result = await unitOfWork.DiscountRepo.GetDiscountById(id);
            var mappedDiscount = mapper.Map<ReadDiscountDetailsDTO>(result);
            return mappedDiscount;
        }
        public async Task CreateDiscount(InsertDiscountDetailsDTO insertDiscountDetailsDTO)
        {
            var mappedDiscount = mapper.Map<Discount>(insertDiscountDetailsDTO);
            var mappedProductDiscounts = mapper.Map<List<ProductDiscount>>(insertDiscountDetailsDTO.InsertProductDiscountDetails);
            foreach(var productDiscount in mappedProductDiscounts)
            {
                productDiscount.Discount = mappedDiscount;
            }
            mappedDiscount.ProductDiscounts = mappedProductDiscounts;
            await unitOfWork.DiscountRepo.CreateDiscount(mappedDiscount);
        }
        public async Task UpdateDiscount(UpdateDiscountDetailsDTO updateDiscountDetailsDTO)
        {
            var mappedDiscount = mapper.Map<Discount>(updateDiscountDetailsDTO);
            var mappedProductDiscounts = mapper.Map<List<ProductDiscount>>(updateDiscountDetailsDTO.UpdateProductDiscountDetails);
            foreach(var productDiscount in mappedProductDiscounts)
            {
                productDiscount.Discount = mappedDiscount;
            }
            mappedDiscount.ProductDiscounts= mappedProductDiscounts;
            await unitOfWork.DiscountRepo.UpdateDiscount(mappedDiscount);
        }
        public async Task DeleteDiscount(int id)
        {
            await unitOfWork.DiscountRepo.DeleteDiscount(id);
        }
        public async Task<List<DiscountDetailsDTO>> GetDiscountsBetween2Dates(DateTime startDate, DateTime endDate)
        {
            var result = await unitOfWork.DiscountRepo.GetDiscountBetween2Dates(startDate,endDate);
            var mappedDiscount = mapper.Map<List<DiscountDetailsDTO>>(result);
            return mappedDiscount;
        }
    }
}
