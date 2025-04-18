using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class InventoryTransaction_factRepos : GenericRepo<TransactionProduct, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public InventoryTransaction_factRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async IAsyncEnumerable<TransactionProduct> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.TransactionProducts
                .Include(b => b.InventoryTransactionHeader)
                .ThenInclude(th => th.TransactionDetail)
            .ThenInclude(td => td.User)
    .AsNoTracking()

                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        #region old
        //public async override Task<List<TransactionDetail>> GetAll()
        //{
        //    return await _blinkDbContext.TransactionDetails
        //        .Include(b => b.InventoryTransactionHeader)

        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }

}
