using System.Collections.Generic;
using AutoMapper;
using Blink_API.DTOs.ProductDTOs;
using Blink_API.Errors;
using Blink_API.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.OpenApi.Any;
using static System.Net.Mime.MediaTypeNames;

namespace Blink_API.Services.Product
{
    public class ProductService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        private readonly IWebHostEnvironment _IWebHostEnvironment;
        public ProductService(UnitOfWork _unitOfWork,IMapper _mapper, IWebHostEnvironment iWebHostEnvironment)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
            _IWebHostEnvironment = iWebHostEnvironment;
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
            int ProductId = await unitOfWork.ProductRepo.AddProduct(product);
            List<InsertProductImagesDTO> ProductImageList = await CheckImagesToSaveInInsert(ProductId, productDTO.ProductImages);
            await AddProductImage(ProductImageList);
            return ProductId;
        }
        public async Task Update(int id, UpdateProductDTO productDTO)
        {
            if(productDTO == null)
                return;
            var product = mapper.Map<Models.Product>(productDTO);
            int ProductId = id;
            List<InsertProductImagesDTO> ProductImageList = await CheckImagesToSaveInUpdate(id,productDTO.NewProductImages,productDTO.OldProductImages);
            await unitOfWork.ProductRepo.UpdateProduct(id, product);
            await AddProductImage(ProductImageList);
        }
        public async Task Delete(int id)
        {
            if (id <= 0)
                return;
            await unitOfWork.ProductRepo.Delete(id);
        }
        public async Task AddProductImage(List<InsertProductImagesDTO> productImagesDTO)
        {
            var prdImages = mapper.Map<List<ProductImage>>(productImagesDTO);
            await unitOfWork.ProductRepo.AddProductImage(prdImages);
        }
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/products");
            if(!File.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);  
            using(var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"wwwroot/images/products/{uniqueFileName}";
        }
        private async Task<List<InsertProductImagesDTO>> CheckImagesToSaveInInsert(int id, List<IFormFile> images)
        {
            var result = new List<InsertProductImagesDTO>();
            foreach(var img in images)
            {
                if(img is IFormFile file)
                {
                    string path = await SaveFileAsync(file);
                    result.Add(new InsertProductImagesDTO { ProductId = id, ProductImagePath = path });
                }
            }
            return result;
        }
        private async Task<List<InsertProductImagesDTO>> CheckImagesToSaveInUpdate(int id,List<IFormFile> newImages,List<string> oldImages)
        {
            var result = new List<InsertProductImagesDTO>();
            foreach (var img in oldImages)
            {
                if (img is string file)
                {
                    string fullPath = img;
                    int startIndex = fullPath.IndexOf("/images/");
                    string relativePath = fullPath.Substring(startIndex + 1);
                    string physicalPath = Path.Combine(_IWebHostEnvironment.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));

                    if (File.Exists(physicalPath))
                    {
                        var fromFile = GetFormFileFromDisk(physicalPath);
                        newImages.Add(fromFile);
                    }
                }
            }
            var oldProductImages = await unitOfWork.ProductRepo.GetProductImages(id);
            foreach (ProductImage productImage in oldProductImages)
            {
                string oldFullPath = productImage.ProductImagePath;
                int startIndex = oldFullPath.IndexOf("/images/");
                string relativePath = oldFullPath.Substring(startIndex + 1);
                string physicalPath = Path.Combine(_IWebHostEnvironment.WebRootPath, relativePath.Replace("/", Path.DirectorySeparatorChar.ToString()));
                if (File.Exists(physicalPath))
                {
                    var fromFile = GetFormFileFromDisk(physicalPath);
                    File.Delete(physicalPath);
                }
            }
            foreach (var img in newImages)
            {
                if (img is IFormFile file)
                {
                    string path = await SaveFileAsync(file);
                    result.Add(new InsertProductImagesDTO { ProductId = id, ProductImagePath = path });
                }
            }
            return result;
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
        public async Task<ICollection<ReadFilterAttributesDTO>> GetFilterAttributesAsync()
        {
            var result = await unitOfWork.ProductRepo.GetFilterAttributeAsync();
            var mapped = mapper.Map<ICollection<ReadFilterAttributesDTO>>(result);
            return mapped;
        }
        public async Task<FilterAttributes?> GetFilterAttributeById(int id)
        {
            var result = await unitOfWork.ProductRepo.GetFilterAttributeById(id);
            return result;
        }
        public async Task<ApiResponse> AddFilterAttribute(InsertFilterAttributeDTO filterAttribute)
        {
            var mappedAttribute=mapper.Map<FilterAttributes>(filterAttribute);  
            int AttributeId = await unitOfWork.ProductRepo.AddFilterAttribute(mappedAttribute);
            if (AttributeId == 0)
                throw new Exception("There is an error occured");
            return new ApiResponse(200, "FilterAttribute Saved Success");
        }
        public async Task<ICollection<DefaultAttributes>> GetDefaultAttributesByAttributeId(int id)
        {
            var result = await unitOfWork.ProductRepo.GetDefaultAttributesByAttributeId(id);
            return result;
        }
        public async Task AddDefaultAttribute(InsertDefaultAttributesDTO defaultAttributes)
        {
            var mappedDefaultAttributes=mapper.Map<DefaultAttributes>(defaultAttributes);
            await unitOfWork.ProductRepo.AddDefaultAttribute(mappedDefaultAttributes);
        }
        public async Task AddProductAttribute(ICollection<InsertProductAttributeDTO> insertProductAttributeDTO)
        {
            var mappedProductAttribute = mapper.Map<ICollection<ProductAttributes>>(insertProductAttributeDTO);
            await unitOfWork.ProductRepo.AddProductAttribute(mappedProductAttribute);
        }
        public async Task<ICollection<InsertProductAttributeDTO>> GetProductAttributes(int productId)
        {
            var productAttributes = await unitOfWork.ProductRepo.GetProductAttributes(productId);
            var mappedProductAttributes = mapper.Map<ICollection<InsertProductAttributeDTO>>(productAttributes);
            return mappedProductAttributes;
        }
        public async Task DeleteProductAttributes(int productId)
        {
            await unitOfWork.ProductRepo.DeleteOldProductAttributes(productId);
        }
        public async Task<ICollection<ProductDiscountsDTO>> GetFillteredProducts(ICollection<FilterProductDTO> filterProductsDTO)
        {
            var products = await unitOfWork.ProductRepo.GetFillteredProducts(filterProductsDTO);
            var mappedProducts = mapper.Map<ICollection<ProductDiscountsDTO>>(products);
            return mappedProducts;
        }
    }
}
