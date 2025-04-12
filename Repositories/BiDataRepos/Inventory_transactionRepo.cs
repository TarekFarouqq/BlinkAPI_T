
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Inventory_transactionRepo:GenericRepo<InventoryTransactionHeader, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Inventory_transactionRepo(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async IAsyncEnumerable<InventoryTransactionHeader> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.InventoryTransactionHeaders
                .Include(b => b.Inventories)
                .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        #region old 
        //public async override Task<List<InventoryTransactionHeader>> GetAll()
        //{
        //    return await _blinkDbContext.InventoryTransactionHeaders
        //        .Include(b => b.Inventories)
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion
    }
}
