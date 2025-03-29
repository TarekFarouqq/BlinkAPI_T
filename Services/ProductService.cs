using AutoMapper;
using Blink_API.DTOs.Product;
using Blink_API.UnitOfWorks;

namespace Blink_API.Services
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
        public async Task<ICollection<ProductDetailsDTO>> GetAllProducts()
        {
            var products = await unitOfWork.ProductRepo.GetAll();
            var result = mapper.Map<ICollection<ProductDetailsDTO>>(products);
            return result;
        }
        public async Task<ProductDetailsDTO> GetProductById(int id)
        {
            var product = await unitOfWork.ProductRepo.GetById(id);
            var result = mapper.Map<ProductDetailsDTO>(product);
            return result;
        }
    }
}
