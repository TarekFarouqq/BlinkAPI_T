using System.Configuration;
using Blink_API.DTOs.ProductDTOs;
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
                .Include(p=>p.ReviewSuppliedProductImages)
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
                .Include(p=>p.ReviewSuppliedProductImages)
                .FirstOrDefaultAsync(p => p.RequestId == requestId);
            return product;
        }
        public async Task<int> AddRequestProduct(ReviewSuppliedProduct product)
        {
            if (product != null)
            {
                await db.ReviewSuppliedProducts.AddAsync(product);
                await db.SaveChangesAsync();
                return product.RequestId;
            }
            return 0;
        }
        public async Task AddRequestImages(List<ReviewSuppliedProductImages> images)
        {
            if(images.Count > 0)
            {
                await db.ReviewSuppliedProductImages.AddRangeAsync(images);
                await db.SaveChangesAsync();
            }
        }
        public async Task AddRequestedProductImages(List<ReviewSuppliedProductImages> productImages)
        {
            await db.ReviewSuppliedProductImages.AddRangeAsync(productImages);
            await SaveChanges();
        }
        public async Task<bool> UpdateRequestProduct(int requestId,ReadReviewSuppliedProductDTO model)
        {
            var currentRequest = await db.ReviewSuppliedProducts.FirstOrDefaultAsync(rp=>rp.RequestId==requestId);
            if (currentRequest != null)
            {
                currentRequest.RequestStatus = model.RequestStatus;
                currentRequest.InventoryId = model.InventoryId;
                currentRequest.CategoryId = model.CategoryId;
                currentRequest.BrandId = model.BrandId;
                currentRequest.ProductQuantity = model.ProductQuantity;
                currentRequest.ProductPrice = model.ProductPrice;
                db.Entry(currentRequest).Property(r => r.RequestStatus).IsModified = true;
                db.Entry(currentRequest).Property(r => r.InventoryId).IsModified = true;
                db.Entry(currentRequest).Property(r => r.CategoryId).IsModified = true;
                db.Entry(currentRequest).Property(r => r.BrandId).IsModified = true;
                await db.SaveChangesAsync();
                return true;
            }
            return false;
        }
        public async Task<List<ReviewSuppliedProductImages>> GetRequestImages(int RequestId)
        {
            var images = await db.ReviewSuppliedProductImages.Where(i => i.RequestId == RequestId).ToListAsync();
            return images;
        }
        public async Task DeleteRequestedProductImage(int RequestId)
        {
            var images = await db.ReviewSuppliedProductImages.Where(pi => pi.RequestId == RequestId).ToListAsync();
            db.ReviewSuppliedProductImages.RemoveRange(images);
            await SaveChanges();
        }
    }
}
