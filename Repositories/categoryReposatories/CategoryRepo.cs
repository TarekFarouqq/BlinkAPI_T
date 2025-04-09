using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories
{
    public class CategoryRepo:GenericRepo<Category,int>
    {
        private readonly BlinkDbContext _db;
        public CategoryRepo(BlinkDbContext db):base(db)
        {
            _db = db;
        }
        public async Task<List<Category>> GetParentCategories()
        {
            return await _db.Categories
                .Where(pc=>pc.ParentCategoryId == null)
                .Where(pc=>!pc.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Category>> GetChildCategories()
        {
            return await _db.Categories
                .Where(pc => pc.ParentCategoryId != null)
                .Where(pc => !pc.IsDeleted)
                .ToListAsync();
        }
        public async Task<Category?> GetParentCategoryById(int id)
        {
            return await _db.Categories
                .Where(pc => pc.ParentCategoryId == null && !pc.IsDeleted && pc.CategoryId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Category?> GetChildCategoryById(int id)
        {
            return await _db.Categories
                .Where(pc => pc.ParentCategoryId != null && !pc.IsDeleted && pc.CategoryId == id)
                .FirstOrDefaultAsync();
        }



        public override void Add(Category entity)
        {
            _db.Set<Category>().Add(entity);
            _db.SaveChanges();

        }


        public override async Task Delete(int id)
        {
            var category = await _db.Categories.Include(c => c.SubCategories).FirstOrDefaultAsync(c => c.CategoryId == id);
            if (category != null)
            {
                if (category.SubCategories.Any(sc => !sc.IsDeleted))
                {
                    throw new InvalidOperationException("Cannot delete category with active subcategories.");
                }

                category.IsDeleted = true;
                _db.Categories.Update(category);
                await _db.SaveChangesAsync();
            }
        }


        public async Task UpdateCategoryAsync(Category category)
        {
            var existingCategory = await _db.Categories
                                            .Where(c => c.CategoryId == category.CategoryId)
                                            .FirstOrDefaultAsync();

            if (existingCategory != null)
            {
                existingCategory.CategoryName = category.CategoryName;
                existingCategory.CategoryDescription = category.CategoryDescription;
                existingCategory.CategoryImage = category.CategoryImage;
                existingCategory.ParentCategoryId = category.ParentCategoryId;

                await _db.SaveChangesAsync();
            }
        }




        public async Task<ICollection<Category>> GetChildCategoryByParentId(int id)
        {
            return await _db.Categories
                .Where(pc => pc.ParentCategoryId != null && !pc.IsDeleted && pc.ParentCategoryId == id)
                .ToListAsync();
        }

    }
}
