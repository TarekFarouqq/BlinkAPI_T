using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.ProductRepos
{
    public class ProductTransferRepo:GenericRepo<InventoryTransactionHeader,int>
    {
        public ProductTransferRepo(BlinkDbContext _db)
            : base(_db)
        {
           
        }
        public async Task<List<InventoryTransactionHeader>> GetAllTransactionHeader()
        {
            return await db.InventoryTransactionHeaders
                .Include(td=>td.TransactionDetail)
                .ThenInclude(src=>src.SrcInventory)
                .Include(td=>td.TransactionDetail)
                .ThenInclude(dest=>dest.DistInventory)
                .Include(tp=>tp.TransactionProducts)
                .ThenInclude(p=>p.Product)
                .Where(p=>p.TransactionProducts.Any(p=>!p.Product.IsDeleted))
                .Where(ith=>!ith.IsDeleted && ith.InventoryTransactionType==3)
                .ToListAsync();
        }
        public async Task<InventoryTransactionHeader?> GetTransactionHeaderById(int id)
        {
            return await db.InventoryTransactionHeaders
               .Include(td => td.TransactionDetail)
               .ThenInclude(src => src.SrcInventory)
               .Include(td => td.TransactionDetail)
               .ThenInclude(dest => dest.DistInventory)
               .Include(tp => tp.TransactionProducts)
               .ThenInclude(p => p.Product)
               .Where(p => p.TransactionProducts.Any(p => !p.Product.IsDeleted))
               .Where(ith => !ith.IsDeleted && ith.InventoryTransactionType == 3)
               .FirstOrDefaultAsync(ith => ith.InventoryTransactionHeaderId == id);
        }
        public async Task AddInputInventory(InventoryTransactionHeader inputInventory)
        {
            await db.InventoryTransactionHeaders.AddAsync(inputInventory);

            await SaveChanges();
        }
        public async Task<decimal> DecreaseStock(int ProductId,int InventoryId,int DecreasedQuantity)
        {
            var stock = await db.StockProductInventories
                .FirstOrDefaultAsync(spi=>spi.ProductId==ProductId && spi.InventoryId==InventoryId && !spi.IsDeleted);
            stock.StockQuantity -= DecreasedQuantity;
            if(stock.StockQuantity <= 0)
            {
                stock.IsDeleted = true;
            }
            await SaveChanges();
            return stock.StockUnitPrice;
        }
        public async Task IncreaseStock(int ProductId, int InventoryId, int IncreasedQuantity, decimal price)
        {
            var stock = await db.StockProductInventories
                .FirstOrDefaultAsync(spi => spi.ProductId == ProductId && spi.InventoryId == InventoryId);
            if (stock == null)
            {
                StockProductInventory newStock = new StockProductInventory
                {
                    ProductId = ProductId,
                    InventoryId = InventoryId,
                    StockUnitPrice = price,
                    StockQuantity = IncreasedQuantity
                };
                await db.StockProductInventories.AddAsync(newStock);
            }
            else
            {
                if (stock.IsDeleted)
                {
                    stock.IsDeleted = false;
                }
                stock.StockQuantity += IncreasedQuantity;

            }
            await SaveChanges();
        }
        public async Task<List<TransactionProduct>> GetOldTransactionProductsByTransactionId(int id)
        {
            var result = await db.TransactionProducts.ToListAsync();
            result = result.Where(ti => ti.InventoryTransactionId == id).ToList();
            return result;

        }
        public async Task DeleteOldTransactionProducts(int id)
        {
            var resultProducts = await db.TransactionProducts.Where(hid=>hid.InventoryTransactionId==id).ToListAsync();
            db.TransactionProducts.RemoveRange(resultProducts);
            await SaveChanges();
        }
        public async Task UpdateTransaction(InventoryTransactionHeader updatedModel)
        {
            db.InventoryTransactionHeaders.Update(updatedModel);
            db.Entry(updatedModel.TransactionDetail).State = EntityState.Modified;

            foreach (var product in updatedModel.TransactionProducts)
            {
                db.Entry(product).State = EntityState.Added;
            }

            await SaveChanges();
        }
        public async Task<int> GetTotalPages(int pgSize)
        {
            var count = await db.InventoryTransactionHeaders
               .Where(p => !p.IsDeleted && p.InventoryTransactionType == 3)
               .CountAsync();
            return (int)Math.Ceiling((double)count / pgSize);
        }
        public async Task<List<InventoryTransactionHeader>> GetAllTransactionHeader(int pgNumber,int pgSize)
        {
            return await db.InventoryTransactionHeaders
                .OrderBy(th=>th.InventoryTransactionDate)
                .Include(td => td.TransactionDetail)
                .ThenInclude(src => src.SrcInventory)
                .Include(td => td.TransactionDetail)
                .ThenInclude(dest => dest.DistInventory)
                .Include(tp => tp.TransactionProducts)
                .ThenInclude(p => p.Product)
                .Where(p => p.TransactionProducts.Any(p => !p.Product.IsDeleted))
                .Where(ith => !ith.IsDeleted && ith.InventoryTransactionType == 3)
                .Skip((pgNumber - 1) * pgSize)
                .Take(pgSize)
                .ToListAsync();
        }
    }
}
