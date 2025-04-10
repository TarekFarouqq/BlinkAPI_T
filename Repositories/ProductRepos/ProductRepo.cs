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
        public async Task<int> GetPagesCount(int pgSize)
        {
            var count = await db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .CountAsync();
            return (int)Math.Ceiling((double)count / pgSize);
        }
        public async Task<List<Product>> GetAllPagginated(int pgNumber,int pgSize)
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
                .Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();
        }
        public async Task<ICollection<Product>> GetFilteredProducts(string filter, int pgNumber,int pgSize)
        {
            return await db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted && 
                (p.ProductName.ToLower().Contains(filter.ToLower()) || p.Category.CategoryName.ToLower().Contains(filter.ToLower()) || p.Brand.BrandName.ToLower().Contains(filter.ToLower())))
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages)
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(sip => sip.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(d => d.Discount)
                .Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();
        }
        public override async Task<Product?> GetById(int id)
        {
            return await db.Products
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
        public async Task<int> AddProduct(Product entity)
        {
            if(entity != null)
            {
                await db.Products.AddAsync(entity);
                await SaveChanges();
                return entity.ProductId;
            }
            return 0;
        }
        public async Task UpdateProduct(int id, Product entity)
        {
            var product = await GetById(id);
            if (product != null)
            {
                product.ProductName = entity.ProductName;
                product.ProductDescription = entity.ProductDescription;
                product.ProductCreationDate = entity.ProductCreationDate;
                product.ProductModificationDate = DateTime.Now;
                product.SupplierId = entity.SupplierId;
                product.BrandId = entity.BrandId;
                product.CategoryId = entity.CategoryId;
                await SaveChanges();
            }
        }
        public override async Task Delete(int id)
        {
            var product = await GetById(id);
            if(product != null)
            {
                product.IsDeleted = true;
                product.ProductModificationDate = DateTime.Now;
                await UpdateProduct(id,product);
            }
        }
        public async Task AddProductImage(int prdId,ProductImage prdImage)
        {
            var product = await GetById(prdId);
            if (product != null)
            {
                prdImage.ProductImageId = db.ProductImages.Any() ? db.ProductImages.Max(pi => pi.ProductImageId) + 1 : 1;
                db.ProductImages.Add(prdImage);
                //await SaveChanges();
            }
        }
    }
}
