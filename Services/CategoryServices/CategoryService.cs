using AutoMapper;
using Blink_API.DTOs.Category;
using Blink_API.Repositories;

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
    }
}
