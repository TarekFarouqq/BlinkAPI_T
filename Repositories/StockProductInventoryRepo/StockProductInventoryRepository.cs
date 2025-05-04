using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.StockProductInventoryRepo
{
    public class StockProductInventoryRepository : GenericRepo<StockProductInventory, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        private readonly IHubContext<NotificationHub> _hubContext;


        public StockProductInventoryRepository(BlinkDbContext blinkDbContext, IHubContext<NotificationHub> hubContext)
            : base(blinkDbContext) 
        {
            _hubContext = hubContext;
            _blinkDbContext = blinkDbContext;
        }


 
        public async Task<List<StockProductInventory>> GetAvailableInventoriesForProduct(int productId)
        {
            var inventories = await _blinkDbContext.StockProductInventories
                .Where(i => i.ProductId == productId && i.StockQuantity > 0)
                .Include(i => i.Inventory)
                .Include(p => p.Product)
                .OrderBy(i => i.Inventory.InventoryId)
                .ToListAsync();

            return inventories;
        }

        public async Task CheckStockAndNotify(int productId)
        {
            var product = await _blinkDbContext.Products
                .FirstOrDefaultAsync(p => p.ProductId == productId);

            if (product == null)
                return;

            var stock = await _blinkDbContext.StockProductInventories
                .Where(i => i.ProductId == productId)
                .SumAsync(i => i.StockQuantity);

            if (stock < 5)
            {
                string name = product.ProductName ?? "Unnamed Product";
                await _hubContext.Clients.All.SendAsync("StockLowNotification",
                    $"Stock for {name} is below 5.");
            }
        }
    }
}
