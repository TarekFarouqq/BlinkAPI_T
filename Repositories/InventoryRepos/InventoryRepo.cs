using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.InventoryRepos
{
    public class InventoryRepo : GenericRepo<Inventory, int>
    {
       
        public InventoryRepo(BlinkDbContext db)
            : base(db)
        {
           
        }


        public async Task<int> CountInventoriesForBranch(int branchId)
        {
            return await db.Inventories
                .Where(i => i.BranchId == branchId && !i.IsDeleted)
                .CountAsync();
        }
        public async override Task<List<Inventory>> GetAll()
        {
            return await db.Inventories.Where(b => b.IsDeleted == false).Include(b => b.Branch).ToListAsync();
        }

        public async override Task<Inventory?> GetById(int id)
        {
            return await db.Inventories
                .Where(b => b.InventoryId == id && b.IsDeleted == false)
                .Include(b => b.Branch)
                .FirstOrDefaultAsync();
        }

        public async Task<bool> IsInventoryHasProducts(int id)
        {
            return await db.StockProductInventories
                .Where(p => p.InventoryId == id && p.Product.IsDeleted == false)
                .AnyAsync(); // Fix: Use AnyAsync() instead of Any()
        }
    }
}
