using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class InventoryTransaction_factRepos : GenericRepo<TransactionDetail, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public InventoryTransaction_factRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async IAsyncEnumerable<TransactionDetail> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.TransactionDetails
                .Include(b => b.InventoryTransactionHeader)
                .Where(b => b.IsDeleted == false)
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
