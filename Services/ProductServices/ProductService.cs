using AutoMapper;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;

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
        public async Task<int> GetPagesCount(int pgSize)
        {
            var count = await unitOfWork.ProductRepo.GetPagesCount(pgSize);
            return count;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetAllPagginated(int pgNumber,int pgSize)
        {
            var products = await unitOfWork.ProductRepo.GetAllPagginated(pgNumber, pgSize);
            var result = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return result;
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetFilteredProducts(string filter, int pgNumber, int pgSize)
        {
            var products = await unitOfWork.ProductRepo.GetFilteredProducts(filter, pgNumber, pgSize);
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
        public async Task<int> Add(InsertProductDTO productDTO)
        {
            if (productDTO == null)
                return 0;
            var product = mapper.Map<Models.Product>(productDTO);
            return await unitOfWork.ProductRepo.AddProduct(product);
        }
        public async Task Update(int id, InsertProductDTO productDTO)
        {
            if(productDTO == null)
                return;
            var product = mapper.Map<Models.Product>(productDTO);
            await unitOfWork.ProductRepo.UpdateProduct(id, product);
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                return;
            await unitOfWork.ProductRepo.Delete(id);
        }
        public async Task AddProductImage(int prdId,ICollection<InsertProductImagesDTO> productImagesDTO)
        {
            var prdImages = mapper.Map<ICollection<ProductImage>>(productImagesDTO);
            foreach (ProductImage prdImage in prdImages)
            {
                await unitOfWork.ProductRepo.AddProductImage(prdId, prdImage);
            }
            await unitOfWork.ProductRepo.SaveChanges();
        }
    }
}
