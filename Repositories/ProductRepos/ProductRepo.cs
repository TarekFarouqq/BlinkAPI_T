using Blink_API.DTOs.ProductDTOs;
using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
namespace Blink_API.Repositories
{
    public class ProductRepo : GenericRepo<Product, int>
    {
        public ProductRepo(BlinkDbContext db)
            : base(db)
        {

        }
        public override async Task<List<Product>> GetAll()
        {
            return await db.Products
                 .AsNoTracking()
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && p.ProductDiscounts.Any(pd => !pd.Discount.IsDeleted && !pd.IsDeleted && pd.Discount.DiscountFromDate <= DateTime.UtcNow && pd.Discount.DiscountEndDate >= DateTime.UtcNow))
                .ToListAsync();
        }
        public async Task<int> GetPagesCount(int pgSize)
        {
            var count = await db.Products
                .Where(p => !p.IsDeleted)
                .CountAsync();
            return (int)Math.Ceiling((double)count / pgSize);
        }
        public async Task<int> GetPagesCountWithUser(int pgSize, string UserId)
        {
            if (pgSize <= 0)
                throw new ArgumentException("Page size must be greater than 0.");

            var count = await db.Products
                .Where(p => !p.IsDeleted && p.SupplierId == UserId)
                .CountAsync();

            return (int)Math.Ceiling((double)count / pgSize);
        }
        public async Task<List<Product>> GetAllPagginated(int pgNumber, int pgSize)
        {
            return await db.Products
                .AsNoTracking()
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted)
                .Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();
        }
        public async Task<List<Product>> GetAllPagginatedWithUser(int pgNumber, int pgSize, string UserId)
        {
            return await db.Products
                .AsNoTracking()
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && p.SupplierId == UserId)
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
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
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
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
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
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
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
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
              .ToListAsync();
        }
        public async Task<int> AddProduct(Product entity)
        {
            if (entity != null)
            {
                var newProductName = entity.ProductName.Trim().ToLower();
                var sameProductName = await db.Products
                    .FirstOrDefaultAsync(p => !p.IsDeleted && p.ProductName.Trim().ToLower() == newProductName);
                if (sameProductName == null)
                {
                    await db.Products.AddAsync(entity);
                    await SaveChanges();
                    return entity.ProductId;
                }
            }
            return 0;
        }
        public async Task<bool> UpdateProduct(int id, Product entity)
        {
            var product = await GetById(id);
            if (product == null)
                return false;
            var newProductName = entity.ProductName.Trim().ToLower();
            var sameProductName = await db.Products
                .AnyAsync(p => !p.IsDeleted && p.ProductName.Trim().ToLower() == newProductName && p.ProductId != id);
            if (sameProductName)
            {
                return false;
            }
            product.ProductName = entity.ProductName;
            product.ProductDescription = entity.ProductDescription;
            product.ProductModificationDate = DateTime.Now;
            product.SupplierId = entity.SupplierId;
            product.BrandId = entity.BrandId;
            product.CategoryId = entity.CategoryId;
            await SaveChanges();
            return true;
        }
        public override async Task Delete(int id)
        {
            var product = await GetById(id);
            if (product != null)
            {
                product.IsDeleted = true;
                product.ProductModificationDate = DateTime.Now;
                await UpdateProduct(id, product);
                var cartDetailsContainProducts = await db.CartDetails.Where(cp => cp.ProductId == id && !cp.IsDeleted).ToListAsync();
                if(cartDetailsContainProducts.Count> 0)
                {
                    foreach(var cartDetail in cartDetailsContainProducts)
                    {
                        cartDetail.IsDeleted = true;
                    }
                }
                var productReviews = await db.Reviews.Where(rp=>rp.ProductId == id && !rp.IsDeleted).ToListAsync();
                if(productReviews.Count > 0)
                {
                    foreach(var review in productReviews)
                    {
                        review.IsDeleted = true;
                    }
                }
                await SaveChanges();
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
                    //image.ProductImageId = db.ProductImages.Any() ? db.ProductImages.Max(pi => pi.ProductImageId) + 1 : 1;
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
        public async Task<ICollection<Product>> GetFillteredProducts(Dictionary<int, List<string>> filtersProduct,int pgNumber,decimal fromPrice,decimal toPrice,int rating,int categoryId)
        {
            var query = db.Products
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p =>
                    !p.IsDeleted &&
                    (fromPrice <= 0 || p.StockProductInventories.Average(spi => spi.StockUnitPrice) >= fromPrice) &&
                    (toPrice <= 0 || p.StockProductInventories.Average(spi => spi.StockUnitPrice) <= toPrice) &&
                    (rating <= 0 || p.Reviews.Average(r => r.Rate) >= rating) &&
                    (categoryId <= 0 || p.CategoryId == categoryId || p.Category.ParentCategory.CategoryId == categoryId)
                );
            foreach (var filter in filtersProduct)
            {
                var attributeId = filter.Key;
                var values = filter.Value;
                query = query.Where(p =>
                    p.ProductAttributes.Any(pa =>
                        pa.AttributeId == attributeId && values.Contains(pa.AttributeValue)
                    )
                );
            }
            var result = await query
                .Skip((pgNumber - 1) * 16)
                .Take(16)
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
        public async Task UpdateStockProducts(ICollection<StockProductInventory> newStockProductInventories)
        {
            if (!newStockProductInventories.Any()) return;
            int productId = newStockProductInventories.First().ProductId;
            var existingStockProducts = await db.StockProductInventories
                .Where(sp => sp.ProductId == productId)
                .ToListAsync();
            db.StockProductInventories.RemoveRange(existingStockProducts);
            await SaveChanges();
            await AddStockProducts(newStockProductInventories);
        }
        public async Task<bool> CheckUserAvailableToReview(string userId, int productId)
        {
            return await db.OrderDetails
                         .AsNoTracking()
                         .AnyAsync(od => od.ProductId == productId
                                      && od.OrderHeader.Cart.UserId == userId);
        }
        public async Task<int?> GetProductStockInInventory(int sourceInventoryId, int productId)
        {
            var stock = await db.StockProductInventories
                .Where(s => s.ProductId == productId && s.InventoryId == sourceInventoryId && !s.IsDeleted)
                .Select(s => (int?)s.StockQuantity)
                .FirstOrDefaultAsync();

            return stock; 
        }
        public async Task<List<Brand>> GetListOfBrands()
        {
            return await db.Brands
                .Where(b =>!b.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Category>> GetSubCategories()
        {
            return await db.Categories
                .Where(c=>c.ParentCategoryId != null && !c.IsDeleted)
                .ToListAsync();
        }
        public async Task<List<Inventory>> GetListOfInventory()
        {
            return await db.Inventories
                .Where(i => !i.IsDeleted)
                .ToListAsync();
        }
        public async Task<bool> DeleteProductImage(int productId,string imagePath)
        {
            var productImage = await db.ProductImages.FirstOrDefaultAsync(p=>p.ProductId==productId && p.ProductImagePath==imagePath);
            if(productImage != null)
            {
                db.ProductImages.Remove(productImage);
                await SaveChanges();
                return true;
            }
            return false;
        }
        public async Task<List<ReviewSuppliedProduct>> GetSuppliedProductsByUserID(int pgNumber, string UserId)
        {
            if (UserId != null)
            {
                return await db.ReviewSuppliedProducts
               .Where(rsp => rsp.SupplierId == UserId)
               .Include(p => p.Brand)
               .Include(p => p.Category)
               .Include(p => p.Inventory)
               .Include(p => p.Supplier)
               .Include(p => p.ReviewSuppliedProductImages)
               .Skip((pgNumber - 1) * 16)
               .Take(16)
               .ToListAsync();
            }
            else
            {
                return await db.ReviewSuppliedProducts
               .Include(p => p.Brand)
               .Include(p => p.Category)
               .Include(p => p.Inventory)
               .Include(p => p.Supplier)
               .Include(p => p.ReviewSuppliedProductImages)
               .Skip((pgNumber - 1) * 16)
               .Take(16)
               .ToListAsync();
            }
        }
        public int GetTotalPageForReviewProducts(string UserId)
        {
            int totalRows = 0;
            if (!string.IsNullOrWhiteSpace(UserId))
            {
                totalRows = db.ReviewSuppliedProducts.Count(r => r.SupplierId == UserId);
            }
            else
            {
                totalRows = db.ReviewSuppliedProducts.Count();
            }
            int countItems = 16;
            return (int)Math.Ceiling((double)totalRows / countItems);
        }
        public async Task<List<Product>> SearchProducts(string searchText)
        {
            var result = await db.Products
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && (p.ProductName.Contains(searchText) || p.ProductId.ToString().Contains(searchText)))
                .Skip(0)
                .Take(20)
                .ToListAsync();
            foreach(var product in result)
            {
                product.ProductImages = product.ProductImages.Where(pi => !pi.IsDeleted).ToList();
            }
            return result;
        }
        public async Task<List<Product>> SearchProducts(string searchText,int inventoryId)
        {
            var result = await db.Products
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && (p.ProductName.Contains(searchText) || p.ProductId.ToString().Contains(searchText)) && p.StockProductInventories.Any(spi=>spi.InventoryId==inventoryId))
                .Skip(0)
                .Take(20)
                .ToListAsync();
            foreach (var product in result)
            {
                product.ProductImages = product.ProductImages.Where(pi => !pi.IsDeleted).ToList();
            }
            return result;
        }
        public async Task<List<Product>> GetProductsByBrandId(int brandId)
        {
            var result = await db.Products
                .AsNoTracking()
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && p.BrandId==brandId)
                .ToListAsync();
            foreach (var product in result)
            {
                product.ProductImages = product.ProductImages.Where(pi => !pi.IsDeleted).ToList();
            }
            return result;
        }
        public async Task<List<Product>> GetProductsByCategoryId(int CategoryId)
        {
            var result = await db.Products
                .AsNoTracking()
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages.Where(pi => !pi.IsDeleted))
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Include(r => r.Reviews)
                .ThenInclude(ru => ru.User)
                .Include(spi => spi.StockProductInventories)
                .Include(pd => pd.ProductDiscounts)
                .ThenInclude(dp => dp.Discount)
                .Where(p => !p.IsDeleted && p.CategoryId == CategoryId)
                .ToListAsync();
            foreach (var product in result)
            {
                product.ProductImages = product.ProductImages.Where(pi => !pi.IsDeleted).ToList();
            }
            return result;
        }
        public async Task<List<Product>> GetProductsByInventoryId(int inventoryId)
        {
            return await db.Products
                .AsNoTracking()
                .Include(spi => spi.StockProductInventories)
                .Where(p => !p.IsDeleted && p.StockProductInventories.Any(spi=>spi.InventoryId==inventoryId)).ToListAsync();
        }
    }
}
