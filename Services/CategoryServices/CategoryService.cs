using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.Models;
using Blink_API.Repositories;

namespace Blink_API.Services
{
    public class CategoryService
    {
        private readonly CategoryRepo categoryRepo;
        private readonly IMapper mapper;
        public CategoryService(CategoryRepo _categoryRepo,IMapper _mapper)
        {
            categoryRepo = _categoryRepo;
            mapper= _mapper;    
        }
        public async Task<List<ParentCategoryDTO>> GetParentCategories()
        {
            var categories = await categoryRepo.GetParentCategories(); // Category
            var resultedMapping = mapper.Map<List<ParentCategoryDTO>>(categories);
            return resultedMapping;
        }
        public async Task<List<ChildCategoryDTO>> GetChildCategories()
        {
            var categories = await categoryRepo.GetChildCategories();
            var resultMapping = mapper.Map<List<ChildCategoryDTO>>(categories); 
            return resultMapping;
        }
    }
}
