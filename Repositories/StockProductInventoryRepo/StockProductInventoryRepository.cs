using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.StockProductInventoryRepo
{
    public class StockProductInventoryRepository:GenericRepo<StockProductInventory,int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public StockProductInventoryRepository(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }



        public async Task<List<StockProductInventory>> GetAvailableInventoriesForProduct(int productId)
        {
            return await _blinkDbContext.StockProductInventories
                .Include(sp => sp.Inventory)
                .Where(sp => !sp.IsDeleted && sp.ProductId == productId)
                .ToListAsync();
        }

    }
}
