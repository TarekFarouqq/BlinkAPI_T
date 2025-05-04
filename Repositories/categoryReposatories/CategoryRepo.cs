using System.Threading.Tasks;
using Blink_API.DTOs.OrdersDTO;
using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;

namespace Blink_API.Repositories
{
    public class CategoryRepo:GenericRepo<Category,int>
    {
        public CategoryRepo(BlinkDbContext _db)
            : base(_db)
        {
           
        }

        public async Task<List<Category>> GetParentCategories()
        {
            return await db.Categories
                .Where(pc=>pc.ParentCategoryId == null)
                .Where(pc=>!pc.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Category>> GetChildCategories()
        {
            return await db.Categories
                .Where(pc => pc.ParentCategoryId != null)
                .Where(pc => !pc.IsDeleted)
                .ToListAsync();
        }
        public async Task<Category?> GetParentCategoryById(int id)
        {
            return await db.Categories
                .Where(pc => pc.ParentCategoryId == null && !pc.IsDeleted && pc.CategoryId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<Category?> GetChildCategoryById(int id)
        {
            return await db.Categories
                .Where(pc => pc.ParentCategoryId != null && !pc.IsDeleted && pc.CategoryId == id)
                .FirstOrDefaultAsync();
        }
        public async Task<ICollection<Category>> GetChildCategoryByParentId(int id)
        {
            return await db.Categories
                .Where(pc => pc.ParentCategoryId != null && !pc.IsDeleted && pc.ParentCategoryId == id)
                .ToListAsync();
        }
        public async Task<List<Category>> GetAll(int pgNumber, int pgSize)
        {
            return await db.Categories
                .Where(c => !c.IsDeleted && c.ParentCategoryId == null) 
                .Skip((pgNumber - 1) * pgSize) 
                .Take(pgSize)  
                .Include(c => c.SubCategories) 
                .ToListAsync();
        }
        public override async Task<Category?> GetById(int id)
        {
            return await db.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => !c.IsDeleted && c.CategoryId == id && c.ParentCategoryId == null);
        }
        public async Task<bool> DeleteParentCategory(int id)
        {
            var parentCategory = await db.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == id && c.ParentCategoryId == null && !c.IsDeleted);
            if (parentCategory == null)
                return false;
            bool canDelete = !parentCategory.SubCategories.Any(sc => !sc.IsDeleted);
            if (canDelete)
            {
                parentCategory.IsDeleted = true;
                await db.SaveChangesAsync();
            }
            return canDelete;
        }
        public async Task<bool> DeleteSubCategory(int id)
        {
            var category = await db.Categories
                .Include(p => p.Products)
                .FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted && c.ParentCategoryId != null);

            if (category == null)
                return false;

            bool hasActiveProducts = category.Products.Any(p => !p.IsDeleted);

            if (hasActiveProducts)
                return false;

            category.IsDeleted = true;
            await db.SaveChangesAsync();

            return true;
        }
        public async Task<bool> AddCategory(Category entity)
        {
            if(entity != null)
            {
                await db.AddAsync(entity);
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<bool> UpdateCategoryWithChildren(Category updatedCategory)
        {
            var existingCategory = await db.Categories
                .Include(c => c.SubCategories)
                .FirstOrDefaultAsync(c => c.CategoryId == updatedCategory.CategoryId && !c.IsDeleted);
            if (existingCategory == null)
                return false;
            existingCategory.CategoryName = updatedCategory.CategoryName;
            existingCategory.CategoryDescription = updatedCategory.CategoryDescription;
            existingCategory.CategoryImage = updatedCategory.CategoryImage;
            foreach (var updatedSub in updatedCategory.SubCategories)
            {
                if(updatedSub.CategoryId > 0)
                {
                    // update sub category 
                    var existingSub = existingCategory.SubCategories
                        .FirstOrDefault(sc => sc.CategoryId == updatedSub.CategoryId);
                    if (existingSub != null)
                    {
                        existingSub.CategoryName = updatedSub.CategoryName;
                        existingSub.CategoryDescription = updatedSub.CategoryDescription;
                        existingSub.CategoryImage = updatedSub.CategoryImage;
                    }
                }
                else
                {
                    // create sub category
                    updatedSub.ParentCategoryId = existingCategory.CategoryId;
                    existingCategory.SubCategories.Add(updatedSub);
                }
            }
            db.Categories.Update(existingCategory);
            await SaveChanges();
            return true;
        }
        public async Task<int> GetTotalPages(int pgSize)
        {
            int totalItems = await db.Categories.Where(c => !c.IsDeleted && c.ParentCategoryId == null).CountAsync();
            int totalPages = (int)Math.Ceiling((double)totalItems / pgSize);
            return totalPages;
        }
        public async Task<Category?> GetUpdatedCategoryById(int id)
        {
            return await db.Categories.FirstOrDefaultAsync(c => c.CategoryId == id && !c.IsDeleted);
        }
    }
}
