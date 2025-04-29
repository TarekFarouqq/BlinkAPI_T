using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.DTOs.CategoryDTOs;
using Blink_API.Models;
using Blink_API.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.EntityFrameworkCore.Metadata.Conventions;

namespace Blink_API.Services
{
    public class CategoryService
    {
        private readonly UnitOfWork unitOfWork;
        private readonly IMapper mapper;
        public CategoryService(UnitOfWork _unitOfWork, IMapper _mapper)
        {
            unitOfWork = _unitOfWork;
            mapper= _mapper;    
        }
        public async Task<List<ParentCategoryDTO>> GetParentCategories()
        {
            var categories = await unitOfWork.CategoryRepo.GetParentCategories(); // Category
            var resultedMapping = mapper.Map<List<ParentCategoryDTO>>(categories);
            return resultedMapping;
        }
        public async Task<List<ChildCategoryDTO>> GetChildCategories()
        {
            var categories = await unitOfWork.CategoryRepo.GetChildCategories();
            var resultMapping = mapper.Map<List<ChildCategoryDTO>>(categories); 
            return resultMapping;
        }
        public async Task<ChildCategoryDTO> GetParentCategoryById(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetParentCategoryById(id);
            var resultMapping = mapper.Map<ChildCategoryDTO>(category);
            return resultMapping;
        }
        public async Task<ChildCategoryDTO> GetChildCategoryById(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetChildCategoryById(id);
            var resultMapping= mapper.Map<ChildCategoryDTO>(category);
            return resultMapping;
        }
        public async Task<string> AddedCategory(CreateCategoryDTO dTO)
        {
            if(dTO.ParentCategoryId.HasValue && dTO.ParentCategoryId.Value > 0)
            {
                var parent = await unitOfWork.CategoryRepo.GetById(dTO.ParentCategoryId.Value);

                if (parent == null || parent.IsDeleted)
                {
                    return "ParentCategory not exist or is removed.";
                }

            }

            var categ = mapper.Map<Category>(dTO);
            unitOfWork.CategoryRepo.Add(categ);
            
            return "category is added";
        }
        public async Task<string> SoftDeleteCategory(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetById(id);
            if (category == null || category.IsDeleted)
                return "Category not found or already deleted.";

            await unitOfWork.CategoryRepo.Delete(id);
            return "Category soft deleted successfully.";
        }
        public async Task<string> UpdateCategory(int id, UpdateCategoryDTO dto)
        {
            // Fetch the existing category
            var category = await unitOfWork.CategoryRepo.GetById(id);
            if (category == null || category.IsDeleted)
            {
                return "Category not found or is deleted.";
            }

            // Validate the parent category
            if (dto.ParentCategoryId.HasValue)
            {
                var parentCategory = await unitOfWork.CategoryRepo.GetById(dto.ParentCategoryId.Value);
                if (parentCategory == null || parentCategory.IsDeleted)
                {
                    return "Parent category does not exist or is deleted.";
                }
            }

            // Map DTO to entity and update
            category.CategoryName = dto.CategoryName;
            category.CategoryDescription = dto.CategoryDescription;
            category.CategoryImage = dto.CategoryImage;
            category.ParentCategoryId = dto.ParentCategoryId;

            await unitOfWork.CategoryRepo.UpdateCategoryWithChildren(category);

            return "Category updated successfully.";
        }
        public async Task<ICollection<ChildCategoryDTO>> GetChildCategoryByParentId(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetChildCategoryByParentId(id);
            var resultMapping = mapper.Map<ICollection<ChildCategoryDTO>>(category);
            return resultMapping;
        }
        #region Sprint 3
        public async Task<List<ReadCategoryDTO>> GetAll(int pgNumber, int pgSize)
        {
            var categories = await unitOfWork.CategoryRepo.GetAll(pgNumber,pgSize);
            var resultMapping = mapper.Map<List<ReadCategoryDTO>>(categories);
            return resultMapping;
        }
        public async Task<ReadCategoryDTO> GetById(int id)
        {
            var categories = await unitOfWork.CategoryRepo.GetById(id);
            var resultMapping = mapper.Map<ReadCategoryDTO>(categories);
            return resultMapping;
        }
        public async Task<bool> DeleteParentCategory(int id)
        {
            return await unitOfWork.CategoryRepo.DeleteParentCategory(id);
        }
        public async Task<bool> DeleteChildCategory(int id)
        {
            return await unitOfWork.CategoryRepo.DeleteSubCategory(id);
        }        
        public async Task<bool> AddCategory(InsertCategoryDTO insertCategoryDTO)
        {
            var parentCategory = mapper.Map<Category>(insertCategoryDTO);
            if(insertCategoryDTO.CategoryImage != null)
            {
                parentCategory.CategoryImage = await SaveImageAsync(insertCategoryDTO.CategoryImage);
            }
            parentCategory.SubCategories = new List<Category>();
            if(insertCategoryDTO?.SubCategories!= null)
            {
                foreach (var subDTO in insertCategoryDTO.SubCategories)
                {
                    var subCategory = mapper.Map<Category>(subDTO);
                    subCategory.ParentCategory = parentCategory;
                    if (subDTO.CategoryImage != null)
                    {
                        subCategory.CategoryImage = await SaveImageAsync(subDTO.CategoryImage);
                    }
                    parentCategory.SubCategories.Add(subCategory);
                }
            }
            bool isInserted = await unitOfWork.CategoryRepo.AddCategory(parentCategory);
            return isInserted;
        }
        public async Task<bool> UpdateCategory(UpdateParentCategoryDTO dto)
        {
            var category = mapper.Map<Category>(dto);
            if (dto.NewImage != null && dto.NewImage.Length > 0)
            {
                category.CategoryImage = await SaveImageAsync(dto.NewImage);
            }
            else if (!string.IsNullOrEmpty(dto.OldImage))
            {
                int indexOfLink = dto.OldImage.IndexOf("/images/");
                string imagePath = dto.OldImage.Substring(indexOfLink + 1);
                category.CategoryImage = "wwwroot/" + imagePath;
            }
            List<Category> subCategoriess = new List<Category>();
            if (dto.SubCategories != null && dto.SubCategories.Any())
            {
                foreach (var subDto in dto.SubCategories)
                {
                    var subCategory = mapper.Map<Category>(subDto);
                    subCategory.ParentCategoryId = category.CategoryId;

                    if (subDto.NewImage != null && subDto.NewImage.Length > 0)
                    {
                        string newPath = await SaveImageAsync(subDto.NewImage);
                        if(newPath != null || newPath != string.Empty)
                        {
                            subCategory.CategoryImage = newPath;
                        }
                    }
                    else if (!string.IsNullOrEmpty(subDto.OldImage))
                    {
                        int indexOfLink = subDto.OldImage.IndexOf("/images/");
                        string imagePath = subDto.OldImage.Substring(indexOfLink + 1);
                        subCategory.CategoryImage = "wwwroot/" + imagePath;
                    }
                    subCategoriess.Add(subCategory);
                }
            }
            category.SubCategories = subCategoriess;
            return await unitOfWork.CategoryRepo.UpdateCategoryWithChildren(category);
        }
        public async Task<string> SaveImageAsync(IFormFile imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;
            var folderPath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "images", "category");
            if (!Directory.Exists(folderPath))
                Directory.CreateDirectory(folderPath);
            var uniqueFileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var fullPath = Path.Combine(folderPath, uniqueFileName);
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }
            return  Path.Combine("wwwroot","images", "category", uniqueFileName).Replace("\\", "/");
        }
        public async Task<int> GetTotalPages(int pgSize)
        {
            return await unitOfWork.CategoryRepo.GetTotalPages(pgSize);
        }
        #endregion
    }
}
