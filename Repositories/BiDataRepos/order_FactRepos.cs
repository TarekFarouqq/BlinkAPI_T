using Blink_API.DTOs.BiDataDtos;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class order_FactRepos : GenericRepo<OrderDetail, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public order_FactRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async IAsyncEnumerable<OrderDetail> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.OrderDetails
                .Include(b => b.OrderHeader)
                .Include(b => b.product)
                .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        #region old 
        //public async override Task<List<OrderDetail>> GetAll()
        //{
        //    return await _blinkDbContext.OrderDetails
        //        .Include(b => b.OrderHeader)

        //        .Include(b => b.product)
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }
}
