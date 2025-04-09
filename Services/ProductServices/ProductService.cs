using AutoMapper;
using Blink_API.DTOs.Product;
using Blink_API.DTOs.ProductDTOs;

namespace Blink_API.Services.Product
{
    public class ProductService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public ProductService(UnitOfWork _unitOfWork,IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetAll()
        {
            var products = await unitOfWork.ProductRepo.GetAll();
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
        public async Task<ProductDiscountsDTO> GetById(int id)
        {
            var product = await unitOfWork.ProductRepo.GetById(id);
            var result = mapper.Map<ProductDiscountsDTO>(product);
            return result;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetByChildCategoryId(int id)
        {
            var products = await unitOfWork.ProductRepo.GetByChildCategoryId(id);
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetByParentCategoryId(int id)
        {
            var products = await unitOfWork.ProductRepo.GetByParentCategoryId(id);
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
    }
}
