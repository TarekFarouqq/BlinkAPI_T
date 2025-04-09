using Blink_API.Models;

namespace Blink_API.Repositories.InventoryRepos
{
    public class InventoryRepo : GenericRepo<Inventory, int>
    {
        private readonly BlinkDbContext db;
        public InventoryRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }


    }
}
