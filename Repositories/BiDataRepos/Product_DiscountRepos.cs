using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Product_DiscountRepos:GenericRepo<ProductDiscount, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Product_DiscountRepos(BlinkDbContext _db)
            : base(_db)
        {
            _blinkDbContext = _db;
        }

        public async IAsyncEnumerable<ProductDiscount> GetAllAsStream()
        {
            await foreach (var productDiscount in _blinkDbContext.ProductDiscounts
                  // .Include(b => b.Discount)
                  // .Include(b => b.Product)
                  .AsNoTracking()
                .AsAsyncEnumerable())
            {
                yield return productDiscount;
            }
        }

        #region old
        //public async override Task<List<ProductDiscount>> GetAll()
        //{
        //    return await _blinkDbContext.ProductDiscounts
        //        .Include(b => b.Discount)
        //        .Include(b => b.Product)
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }

}
