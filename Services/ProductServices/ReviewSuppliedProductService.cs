using AutoMapper;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;

namespace Blink_API.Services.ProductServices
{
    public class ReviewSuppliedProductService
    {
        private IMapper mapper;
        private UnitOfWork unitOfWork;
        public ReviewSuppliedProductService(IMapper _mapper, UnitOfWork _unitOfWork)
        {
            mapper = _mapper;
            unitOfWork = _unitOfWork;
        }
        public async Task<List<ReadReviewSuppliedProductDTO>> GetSuppliedProducts()
        {
            var reviewSuppledProducts = await unitOfWork.ProductSupplierRepo.GetSuppliedProducts();
            var mappedReviewSuppliedProducts = mapper.Map<List<ReadReviewSuppliedProductDTO>>(reviewSuppledProducts);
            return mappedReviewSuppliedProducts;
        }
        public async Task<ReadReviewSuppliedProductDTO?> GetSuppliedProductByRequestId(int requestId)
        {
            var reviewSuppliedProduct = await unitOfWork.ProductSupplierRepo.GetSuppliedProductByRequestId(requestId);
            if (reviewSuppliedProduct == null)
            {
                return null;
            }
            var mappedReviewSuppliedProduct = mapper.Map<ReadReviewSuppliedProductDTO>(reviewSuppliedProduct);
            return mappedReviewSuppliedProduct;
        }
        public async Task AddRequestProduct(InsertReviewSuppliedProductDTO insertReviewSuppliedProductDTO)
        {
            
            
            var reviewSuppliedProduct = mapper.Map<ReviewSuppliedProduct>(insertReviewSuppliedProductDTO);
            var requestId = await unitOfWork.ProductSupplierRepo.AddRequestProduct(reviewSuppliedProduct);
            List< ReviewSuppliedProductImages > resultImages = new List<ReviewSuppliedProductImages>();
            foreach (var file in insertReviewSuppliedProductDTO.ProductImages)
            {
                if (file.Length > 0)
                {
                    var filePath = await SaveFileAsync(file);
                    var reviewSupply = new ReviewSuppliedProductImages()
                    {
                        RequestId = requestId,
                        ImagePath = filePath
                    };
                    resultImages.Add(reviewSupply);
                }
            }
            await unitOfWork.ProductSupplierRepo.AddRequestedProductImages(resultImages);


            //return requestId;
        }
        public async Task UpdateRequestProduct(int requestId, bool status)
        {
            await unitOfWork.ProductSupplierRepo.UpdateRequestProduct(requestId, status);
        }
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/tempProducts");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/images/tempProducts/products/{uniqueFileName}";
        }
    }
}
