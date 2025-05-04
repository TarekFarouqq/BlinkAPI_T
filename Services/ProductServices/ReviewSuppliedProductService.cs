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
            var mappedReview = mapper.Map<ReviewSuppliedProduct>(insertReviewSuppliedProductDTO);
            int requestId = await unitOfWork.ProductSupplierRepo.AddRequestProduct(mappedReview);
            if (requestId > 0)
            {
                List<ReviewSuppliedProductImages> resultImages = new List<ReviewSuppliedProductImages>();
                foreach (var file in insertReviewSuppliedProductDTO.ProductImages)
                {   
                    if (file.Length > 0)
                    {
                        var filePath = await SaveFileAsync(file);
                        var reviewSupply = new ReviewSuppliedProductImages()
                        {
                            RequestId = requestId, // Set the generated ID
                            ImagePath = filePath
                        };
                        resultImages.Add(reviewSupply);
                    }
                }
                await unitOfWork.ProductSupplierRepo.AddRequestImages(resultImages);
            }
        }
        public async Task UpdateRequestProduct(int requestId, ReadReviewSuppliedProductDTO model)
        {
            var mappedModel = mapper.Map<ReviewSuppliedProduct>(model);
            bool isUpdated = await unitOfWork.ProductSupplierRepo.UpdateRequestProduct(requestId, model);
            if (isUpdated)
            {
                if (Convert.ToBoolean(model.RequestStatus))
                {
                    //adding product
                    Models.Product newProduct = mapper.Map<Models.Product>(mappedModel);
                    int NewProductId = await unitOfWork.ProductRepo.AddProduct(newProduct);
                    var requestedImages = await unitOfWork.ProductSupplierRepo.GetRequestImages(model.RequestId);

                    //adding product images
                    List<ProductImage> newImageList = new List<ProductImage>();
                    foreach (var image in requestedImages)
                    {
                        IFormFile file = GetFormFileFromDisk2(image.ImagePath);
                        string savedPath = await SaveAcceptedProducts(file);
                        var newImage = new ProductImage()
                        {
                            ProductId = NewProductId,
                            ProductImagePath = savedPath
                        };
                        newImageList.Add(newImage);

                    }
                    await unitOfWork.ProductRepo.AddProductImage(newImageList);

                    //adding product stock and price 
                    var stockEntry = new StockProductInventory
                    {
                        ProductId = NewProductId,
                        InventoryId = model.InventoryId,
                        StockUnitPrice = (decimal)model.ProductPrice, 
                        StockQuantity = model.ProductQuantity,       
                    };
                    unitOfWork.StockProductInventoryRepo.Add(stockEntry);
                    await unitOfWork.CompleteAsync();


                }
            }
        }
        public IFormFile GetFormFileFromDisk(string path)
        {
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path).ToLower();
            string contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };
            byte[] fileBytes = File.ReadAllBytes(path);
            var stream = new MemoryStream(fileBytes);

            return new FormFile(stream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };

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
            return $"/images/tempProducts/{uniqueFileName}";
        }
        private async Task<string> SaveAcceptedProducts(IFormFile file)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if (!Directory.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/images/products/{uniqueFileName}";
        }
        public IFormFile GetFormFileFromDisk2(string relativePath)
        {
            if (relativePath.StartsWith("/"))
                relativePath = relativePath.Substring(1);

            string fullPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

            if (!File.Exists(fullPath))
                throw new FileNotFoundException($"File not found at path: {fullPath}");

            var fileName = Path.GetFileName(fullPath);
            var extension = Path.GetExtension(fullPath).ToLower();
            string contentType = extension switch
            {
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                _ => "application/octet-stream"
            };

            byte[] fileBytes = File.ReadAllBytes(fullPath);
            var stream = new MemoryStream(fileBytes);

            return new FormFile(stream, 0, fileBytes.Length, "file", fileName)
            {
                Headers = new HeaderDictionary(),
                ContentType = contentType
            };
        }
        
    }
}
