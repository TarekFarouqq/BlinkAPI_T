using Blink_API.DTOs.ProductDTOs;
using Blink_API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Diagnostics;
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
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
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
        public async Task<List<Product>> GetAllPagginated(int pgNumber, int pgSize)
        {
            return await db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(sip => sip.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(d => d.Discount)
                .Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();
        }
        public async Task<ICollection<Product>> GetFilteredProducts(string filter, int pgNumber, int pgSize)
        {
            return await db.Products
                .AsNoTracking()
                .Where(p => !p.IsDeleted &&
                (p.ProductName.ToLower().Contains(filter.ToLower()) || p.Category.CategoryName.ToLower().Contains(filter.ToLower()) || p.Brand.BrandName.ToLower().Contains(filter.ToLower())))
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
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
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
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
              .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
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
              .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
              .Include(r => r.Reviews)
              .ThenInclude(rc => rc.ReviewComments)
              .Include(sip => sip.StockProductInventories)
              .Include(pd => pd.ProductDiscounts)
              .ThenInclude(d => d.Discount)
              .ToListAsync();
        }
        public async Task<int> AddProduct(Product entity)
        {
            if (entity != null)
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
            if (product != null)
            {
                product.IsDeleted = true;
                product.ProductModificationDate = DateTime.Now;
                await UpdateProduct(id, product);
            }
        }
        private async Task RemoveOldImages(int prdId)
        {
            var product = await GetById(prdId);
            if (product != null)
            {
                var oldImages = db.ProductImages
                    .Where(pi => pi.ProductId == prdId)
                    .ToList();
                foreach (ProductImage img in oldImages)
                {
                    img.IsDeleted = true;
                }
                await db.SaveChangesAsync();
            }
        }
        public async Task AddProductImage(List<ProductImage> prdImages)
        {
            if (prdImages.Count == 0)
                return;
            var product = await GetById(prdImages[0].ProductId);
            if (product != null)
            {
                await RemoveOldImages(product.ProductId);

                foreach (ProductImage image in prdImages)
                {
                    image.ProductImageId = db.ProductImages.Any() ? db.ProductImages.Max(pi => pi.ProductImageId) + 1 : 1;
                    db.ProductImages.Add(image);
                    await SaveChanges();
                }

            }
        }
        public async Task<ICollection<FilterAttributes>> GetFilterAttributeAsync()
        {
            return await db.FilterAttributes
                .AsNoTracking()
                .Include(fa => fa.DefaultAttributes)
                .ToListAsync();
        }
        public async Task<FilterAttributes?> GetFilterAttributeById(int id)
        {
            return await db.FilterAttributes.FirstOrDefaultAsync(fa => fa.AttributeId == id);
        }
        public async Task<int> AddFilterAttribute(FilterAttributes filterAttribute)
        {
            await db.FilterAttributes.AddAsync(filterAttribute);
            await SaveChanges();
            return filterAttribute.AttributeId;
        }
        public async Task<ICollection<DefaultAttributes>> GetDefaultAttributesByAttributeId(int id)
        {
            return await db.DefaultAttributes
                .AsNoTracking()
                .Where(da => da.AttributeId == id)
                .ToListAsync();
        }
        public async Task AddDefaultAttribute(DefaultAttributes defaultAttributes)
        {
            await db.DefaultAttributes.AddAsync(defaultAttributes);
            await SaveChanges();
        }
        public async Task DeleteOldProductAttributes(int prdId)
        {
            var oldAttributes = await db.ProductAttributes
                .Where(pa=>pa.ProductId==prdId)
                .ToListAsync();
            db.ProductAttributes.RemoveRange(oldAttributes);
            await SaveChanges();
        }
        public async Task AddProductAttribute(ICollection<ProductAttributes> productAttributes)
        {
            if (productAttributes == null || productAttributes.Count == 0)
                return;
            await DeleteOldProductAttributes(productAttributes.ToList()[0].ProductId);
            await db.ProductAttributes.AddRangeAsync(productAttributes);
            await SaveChanges();
        }
        public async Task<ICollection<ProductAttributes>> GetProductAttributes(int productId)
        {
            return await db.ProductAttributes
                .AsNoTracking()
                .Where(pa => pa.ProductId == productId)
                .ToListAsync();
        }
        public async Task<List<ProductImage>> GetProductImages(int ProductId)
        {
            return await db.ProductImages
                .AsNoTracking()
                .Where(p => p.ProductId == ProductId && !p.IsDeleted)
                .ToListAsync();
        }
        public async Task DeleteOldProductImages(int productId)
        {
            var oldImages = await db.ProductImages
                .Where(pi => pi.ProductId == productId)
                .ToListAsync();
            db.ProductImages.RemoveRange(oldImages);
            await SaveChanges();
        }
        public async Task<ICollection<Product>> GetFillteredProducts([FromQuery] Dictionary<int, List<string>> filtersProduct,int pgNumber)
        {
            var resultProduct = db.Products
                .AsNoTracking()
                .Include(p => p.ProductAttributes)
                .ThenInclude(fa => fa.FilterAttribute)
                .Where(p => !p.IsDeleted)
                .AsQueryable();
            foreach(var filter in filtersProduct)
            {
                int AttributeId = filter.Key;
                List<string> attributesValue = filter.Value;
                foreach(var attribute in attributesValue)
                {
                    resultProduct = resultProduct.Where(p => p.ProductAttributes.Any(
                            pa=>pa.AttributeId==AttributeId && pa.AttributeValue==attribute
                        ));
                }
            }
            var result = await resultProduct
                .Where(p => !p.IsDeleted)
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(sip => sip.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(d => d.Discount)
                .Skip((pgNumber - 1) * 16)
                .ToListAsync();
            return result;
        }
        public async Task<ICollection<StockProductInventory>> GetProductStock(int productId)
        {
            return await db.StockProductInventories
                .AsNoTracking()
                .Where(sp => sp.ProductId == productId && !sp.IsDeleted)
                .ToListAsync();
        }
        public async Task AddStockProducts(ICollection<StockProductInventory> stockProductInventories)
        {
            if (stockProductInventories == null || stockProductInventories.Count == 0)
                return;
            await db.StockProductInventories.AddRangeAsync(stockProductInventories);
            await SaveChanges();
        }
        public async Task UpdateStockProducts(ICollection<StockProductInventory> stockProductInventories)
        {
            db.StockProductInventories.RemoveRange(stockProductInventories);
            await SaveChanges();
            await AddStockProducts(stockProductInventories);
        }

        // count products to check brand:
        //public async Task <int> productsCount(int brandId)
        //{
        //    return await db.Products.Where(p=>p.BrandId == brandId).CountAsync();
        //}


        
    }
}
