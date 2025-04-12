using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class BiDataRepos:GenericRepo<StockProductInventory,int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public BiDataRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async override Task<List<StockProductInventory>> GetAll()
        {
            return await _blinkDbContext.StockProductInventories
                .Include(b => b.Inventory)
                .Include(b => b.Product)
                .Where(b => b.IsDeleted == false)
                .ToListAsync();
        }

    }
}
