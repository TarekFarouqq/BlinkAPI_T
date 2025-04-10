using AutoMapper;
using Blink_API.DTOs.BranchDto;
using Blink_API.DTOs.BrandDtos;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Repositories;

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

        // display brands :
        public async Task<ICollection<BrandDTO>> GetAllBrands()
        {
            var brands = await unitOfWork.BrandRepos.GetAll();
            var result = mapper.Map<ICollection<BrandDTO>>(brands);
            return result;
        }
        // get by id :
        public async Task<BrandDTO?> GetBrandbyId(int id)
        {
            var brand = await unitOfWork.BrandRepos.GetById(id);
            if (brand == null) return null;
            var brandDto = mapper.Map<BrandDTO>(brand);  
            return brandDto;
        }
        // get by name :
        public async Task<ICollection<BrandDTO>> GetBrandByName(string name)
        {
            var brands = await unitOfWork.BrandRepos.GetByName(name);
            if (brands == null || !brands.Any()) return new List<BrandDTO>();  
            var brandsDto = mapper.Map<ICollection<BrandDTO>>(brands);
            return brandsDto;
        }


        //// insert || add brand 
        public async Task<ApiResponse> InsertBrand(insertBrandDTO insertedBrand)
        {
            if (insertedBrand == null)
            {
                throw new ArgumentException("Invalid brand, please try again ! ");  
            }
            var brand = mapper.Map<Brand>(insertedBrand);
            unitOfWork.BrandRepos.Add(brand);
            await unitOfWork.BrandRepos.SaveChanges();
            //return mapper.Map<BrandDTO>(brand);
            return new ApiResponse(201, "Brand added successfully.");
        }

        //// update brand :
        public async Task<ApiResponse> UpdateBrand(int id, insertBrandDTO updateBrand)
        {
            if (updateBrand == null)
            {
                throw new ArgumentException("Invalid brand, please try again ! ");
            }

            var brand = await unitOfWork.BrandRepos.GetById(id);
            if (brand == null)
            {
                throw new Exception("Cant find this brand");
            }
            brand.BrandName = updateBrand.BrandName;
            brand.BrandDescription = updateBrand.BrandDescription;
            brand.BrandImage = updateBrand.BrandImage;
            brand.BrandWebSiteURL = updateBrand.BrandWebSiteURL;

            await unitOfWork.BrandRepos.SaveChanges();
            return new ApiResponse(200, "Brand updated successfully.");
        }

        //// soft delete brand :
        public async Task<ApiResponse> SoftDeleteBrand(int id)
        {
            var brand = await unitOfWork.BrandRepos.GetById(id);
            if (brand == null || brand.IsDeleted)
            {
                throw new Exception("Caant found this brand");
            }
            await unitOfWork.BrandRepos.Delete(id);
            await unitOfWork.BrandRepos.SaveChanges();
            return new ApiResponse(200, "Brand deleted successfully.");
        }
    }
}
