using AutoMapper;
using Blink_API.DTOs.BrandDtos;
using Blink_API.Errors;
using Blink_API.Models;
namespace Blink_API.Services.BrandServices
{
    public class BrandService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public BrandService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper = _mapper;
        }
        public async Task<ICollection<BrandDTO>> GetAllBrands()
        {
            var brands = await unitOfWork.BrandRepos.GetAll();
            ICollection<BrandDTO> result = mapper.Map<ICollection<BrandDTO>>(brands);
            foreach (BrandDTO brand in result)
            {
                string fullPath = brand.BrandImage;
                int startIndex = fullPath.IndexOf("/images/");
                brand.BrandImage = fullPath.Substring(startIndex + 1);
            }
            return result;
        }
        public async Task<BrandDTO?> GetBrandbyId(int id)
        {
            var brand = await unitOfWork.BrandRepos.GetById(id);
            if (brand == null) return null;
            var brandDto = mapper.Map<BrandDTO>(brand);
            string fullPath = brandDto.BrandImage;
            int startIndex = fullPath.IndexOf("/images/");
            brandDto.BrandImage = fullPath.Substring(startIndex + 1);
            return brandDto;
        }
        public async Task<ICollection<BrandDTO>> GetBrandByName(string name)
        {
            var brands = await unitOfWork.BrandRepos.GetByName(name);
            if (brands == null || !brands.Any()) return new List<BrandDTO>();  
            var brandsDto = mapper.Map<ICollection<BrandDTO>>(brands);
            foreach (BrandDTO brand in brandsDto)
            {
                string fullPath = brand.BrandImage;
                int startIndex = fullPath.IndexOf("/images/");
                brand.BrandImage = fullPath.Substring(startIndex + 1);
            }
            return brandsDto;
        }
        public async Task<ApiResponse> InsertBrand(insertBrandDTO insertedBrand)
        {
            if (insertedBrand == null)
            {
                throw new ArgumentException("Invalid brand, please try again ! ");  
            }
            var savedPath = await SaveFileAsync(insertedBrand.BrandImageFile);
            var brand = mapper.Map<Brand>(insertedBrand);
            brand.BrandImage = savedPath;
            unitOfWork.BrandRepos.Add(brand);
            await unitOfWork.BrandRepos.SaveChanges();
            return new ApiResponse(200, "Brand added successfully.");
        }
        public async Task<ApiResponse> UpdateBrand(int id, insertBrandDTO updateBrand)
        {
            if(updateBrand == null)
                throw new ArgumentException("Invalid brand, please try again ! ");
            var brand = await unitOfWork.BrandRepos.GetById(id);
            if(brand == null)
                throw new Exception("Cant find this brand");
            await DeleteOldImageFromAPI(id);
            var savedPath = await SaveFileAsync(updateBrand.BrandImageFile);
            var mappedBrand = mapper.Map<Brand>(updateBrand);
            mappedBrand.BrandImage = savedPath;
            await unitOfWork.BrandRepos.UpdateBrand(id, mappedBrand);
            return new ApiResponse(200, "Brand Successfull Updated");
        }
        public async Task<ApiResponse> SoftDeleteBrand(int id)
        {
            if (id <= 0)
                throw new Exception("Brand Id should be more than 0");
            var brand = await unitOfWork.BrandRepos.GetById(id);
            if (brand == null)
                throw new Exception("Brand Not Found");
            bool isDeleted = await unitOfWork.BrandRepos.SoftDeleteBrand(id);
            if (isDeleted)
            {
                return new ApiResponse(200, "Brand Successfull Deleted");
            }
            else
            {
                return new ApiResponse(409, "Brand Can't Delete Because there is an Products Related to it");
            }
        }
        private async Task<string> SaveFileAsync(IFormFile file)
        {
            var uploadFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/images/brands");
            if (!File.Exists(uploadFolder))
                Directory.CreateDirectory(uploadFolder);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
            var filePath = Path.Combine(uploadFolder, uniqueFileName);
            using (var fileStream = new FileStream(filePath, FileMode.Create))
            {
                await file.CopyToAsync(fileStream);
            }
            return $"/images/brands/{uniqueFileName}";
        }
        private async Task DeleteOldImageFromAPI(int brandId)
        {
            var oldBrand = await GetBrandbyId(brandId);
            if(oldBrand != null)
            {
                string currentImagePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", oldBrand.BrandImage);
                if (File.Exists(currentImagePath))
                {
                    File.Delete(currentImagePath);
                }
            }
        }
    }
}