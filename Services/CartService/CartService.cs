using AutoMapper;
using Blink_API.DTOs.CartDTOs;
using Blink_API.DTOs.Category;

namespace Blink_API.Services.CartService
{
    public class CartService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CartService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }

        public async Task<List<ReadCartDTO>> GetAllCarts()
        {
            var carts = await unitOfWork.CartRepo.GetAll(); 
            var resultedMapping = mapper.Map<List<ReadCartDTO>>(carts);
            return resultedMapping;
        }

        public async Task<ReadCartDTO> GetByUserId(string id)
        {
            var cart = await unitOfWork.CartRepo.GetByUserId(id);
            var resultedMapping = mapper.Map<ReadCartDTO>(cart);
            return resultedMapping;
        }
    }
}
