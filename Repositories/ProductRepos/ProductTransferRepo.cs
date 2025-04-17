using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.ProductRepos
{
    public class ProductTransferRepo:GenericRepo<InventoryTransactionHeader,int>
    {
        public ProductTransferRepo(BlinkDbContext db):base(db){}
        public async Task<List<InventoryTransactionHeader>> GetAllTransactionHeader()
        {
            return await db.InventoryTransactionHeaders
                .Include(td=>td.TransactionDetail)
                .Include(tp=>tp.TransactionProducts)
                .Include(i=>i.Inventories)
                .Where(ith=>!ith.IsDeleted)
                .ToListAsync();
        }
    }
}
