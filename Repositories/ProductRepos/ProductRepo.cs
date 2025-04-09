using System.Runtime.CompilerServices;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories
{
    public class ProductRepo : GenericRepo<Product, int>
    {
        private readonly BlinkDbContext db;
        public ProductRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }
        public override async Task<List<Product>> GetAll()
        {
            return await db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages)
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(sip => sip.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(d => d.Discount)
                .ToListAsync();
        }
        public override async Task<Product?> GetById(int id)
        {
            return await db.Products
                .AsNoTracking()
               .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages)
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(sip => sip.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(d => d.Discount)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
        public async Task<ICollection<Product>> GetByChildCategoryId(int categoryId)
        {
            return await db.Products
              .AsNoTracking()
              .Where(p => !p.IsDeleted && p.CategoryId == categoryId && p.Category.ParentCategoryId != null)
              .Include(u => u.User)
              .Include(b => b.Brand)
              .Include(c => c.Category)
              .Include(i => i.ProductImages)
              .Include(r => r.Reviews)
              .ThenInclude(rc => rc.ReviewComments)
              .Include(sip => sip.StockProductInventories)
              .Include(pd => pd.ProductDiscounts)
              .ThenInclude(d => d.Discount)
              .ToListAsync();
        }
        public async Task<ICollection<Product>> GetByParentCategoryId(int categoryId)
        {
            return await db.Products
              .AsNoTracking()
              .Where(p => !p.IsDeleted && p.Category.ParentCategoryId == categoryId)
              .Include(u => u.User)
              .Include(b => b.Brand)
              .Include(c => c.Category)
              .Include(i => i.ProductImages)
              .Include(r => r.Reviews)
              .ThenInclude(rc => rc.ReviewComments)
              .Include(sip => sip.StockProductInventories)
              .Include(pd => pd.ProductDiscounts)
              .ThenInclude(d => d.Discount)
              .ToListAsync();
        }
    }
}
