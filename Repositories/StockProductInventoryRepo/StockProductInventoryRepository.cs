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
            var inventories = await _blinkDbContext.StockProductInventories
                   .Where(i => i.ProductId == productId && i.StockQuantity > 0)
                   .OrderBy(i => i.Inventory.InventoryId)
                    
                   .ToListAsync();

            return inventories;
        }

    }
}
