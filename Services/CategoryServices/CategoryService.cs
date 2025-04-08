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
        public async Task<ICollection<ChildCategoryDTO>> GetChildCategoryByParentId(int id)
        {
            var category = await unitOfWork.CategoryRepo.GetChildCategoryByParentId(id);
            var resultMapping = mapper.Map<ICollection<ChildCategoryDTO>>(category);
            return resultMapping;
        }
    }
}
