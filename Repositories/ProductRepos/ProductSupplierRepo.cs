using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.ProductRepos
{
    public class ProductSupplierRepo: GenericRepo<ReviewSuppliedProduct, int>
    {
        private BlinkDbContext db;
        public ProductSupplierRepo(BlinkDbContext _db):base(_db)
        {
            db = _db;
        }
        public async Task<List<ReviewSuppliedProduct>> GetSuppliedProducts()
        {
            var products = await db.ReviewSuppliedProducts
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .Include(p => p.Supplier)
                .ToListAsync();
            return products;
        }
        public async Task<ReviewSuppliedProduct?> GetSuppliedProductByRequestId(int requestId)
        {
            var product = await db.ReviewSuppliedProducts
                .Include(p => p.Brand)
                .Include(p => p.Category)
                .Include(p => p.Inventory)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(p => p.RequestId == requestId);
            return product;
        }
        public async Task<int> AddRequestProduct(ReviewSuppliedProduct product)
        {
            await db.ReviewSuppliedProducts.AddAsync(product);
            await db.SaveChangesAsync();
            return product.RequestId;
        }
        public async Task AddRequestedProductImages(List<ReviewSuppliedProductImages> productImages)
        {
            await db.ReviewSuppliedProductImages.AddRangeAsync(productImages);
            await SaveChanges();
        }
        public async Task UpdateRequestProduct(int requestId,bool status)
        {
            var requestProduct = await db.ReviewSuppliedProducts.FirstOrDefaultAsync(rp=>rp.RequestId == requestId);
            if (requestProduct != null)
            {
                requestProduct.RequestStatus = status;
                db.ReviewSuppliedProducts.Update(requestProduct);
            }
            await db.SaveChangesAsync();
        }
    }
}
