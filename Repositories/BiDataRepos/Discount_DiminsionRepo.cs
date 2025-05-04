using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Discount_DiminsionRepo : GenericRepo<ProductDiscount, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Discount_DiminsionRepo(BlinkDbContext _db)
            : base(_db)
        {
            _blinkDbContext = _db;
        }

        public async IAsyncEnumerable<ProductDiscount> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.ProductDiscounts
                 .Include(pd => pd.Discount)
                // .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }


        #region old
        //public async override Task<List<Discount>> GetAll()
        //{
        //    return await _blinkDbContext.Discounts
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }
}
