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
        public async Task<ICollection<ProductDetailsDTO>> GetAllProducts()
        {
            var products = await unitOfWork.ProductRepo.GetAllAsync();
            var result = mapper.Map<ICollection<ProductDetailsDTO>>(products);
            return result;
        }
        public async Task<ProductDetailsDTO> GetProductById(int id)
        {
            var product = await unitOfWork.ProductRepo.GetById(id);
            var result = mapper.Map<ProductDetailsDTO>(product);
            return result;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetProductsWithRunningDiscounts()
        {
            var products=await unitOfWork.ProductRepo.GetProductsWithRunningDiscounts();
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
        public async Task<ProductDiscountsDTO> GetProductWithRunningDiscountByProductId(int id)
        {
            var product = await unitOfWork.ProductRepo.GetProductWithRunningDiscountByProductId(id);
            var result = mapper.Map<ProductDiscountsDTO>(product);
            return result;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetProductsWithCategoryId(int categoryId)
        {
            var products = await unitOfWork.ProductRepo.GetProductsWithCategoryId(categoryId);
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
    }
}
