using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class cart_DiminsionRepos:GenericRepo<Cart, int>

    {
        private readonly BlinkDbContext _blinkDbContext;
        public cart_DiminsionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async IAsyncEnumerable<Cart> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.Carts
                .Include(b => b.CartDetails)
                .ThenInclude(b => b.Product)
                .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        #region old
        //public async override Task<List<Cart>> GetAll()
        //{
        //    return await _blinkDbContext.Carts
        //        .Include(b => b.CartDetails)
        //        .ThenInclude(b => b.Product)
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }
}
