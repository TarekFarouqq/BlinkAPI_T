using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.DTOs.CategoryDTOs;
using Blink_API.Models;
using Blink_API.Repositories;
using Microsoft.AspNetCore.Mvc.Formatters;

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
        public async Task<ChildCategoryDTO> GetChildCategoryById(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetChildCategoryById(id);
            var resultMapping= mapper.Map<ChildCategoryDTO>(category);
            return resultMapping;
        }


        //adding category
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

            await unitOfWork.CategoryRepo.UpdateCategoryAsync(category);

            return "Category updated successfully.";
        }


    }
}
