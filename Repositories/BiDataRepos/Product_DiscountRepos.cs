using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Product_DiscountRepos:GenericRepo<ProductDiscount, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Product_DiscountRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async IAsyncEnumerable<ProductDiscount> GetAllAsStream()
        {
            await foreach (var productDiscount in _blinkDbContext.ProductDiscounts
                .Include(b => b.Discount)
                .Include(b => b.Product)
                .Where(b => b.IsDeleted == false)
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
