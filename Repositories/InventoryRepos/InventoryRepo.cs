using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.InventoryRepos
{
    public class InventoryRepo : GenericRepo<Inventory, int>
    {
        private readonly BlinkDbContext db;
        public InventoryRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
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
    }
}
