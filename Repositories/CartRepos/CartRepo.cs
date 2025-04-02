using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.CartRepos
{
    public class CartRepo : GenericRepo<Cart, int>
    {
        private readonly BlinkDbContext db;
        public CartRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }

        public override async Task<List<Cart>> GetAll()
        {
            return await db.Carts
                .AsNoTracking()
                    .Include(c=> c.CartDetails)
                         .ThenInclude(c=> c.Product).ThenInclude(c=>c.StockProductInventories)
                    .Include(c => c.CartDetails)
                         .ThenInclude(c => c.Product).ThenInclude(c=>c.ProductImages)
                
                .ToListAsync();
        }

        public async Task<Cart?> GetByUserId(string id)
        {
            return await db.Carts
                .AsNoTracking()
                 .Include(c => c.CartDetails)
                         .ThenInclude(c => c.Product).ThenInclude(c => c.StockProductInventories)
                    .Include(c => c.CartDetails)
                         .ThenInclude(c => c.Product).ThenInclude(c => c.ProductImages)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<int?> AddCart(string id)
        {
            
            var cart = await GetByUserId(id);

            if (cart == null)
            {
                cart = new Cart() { UserId = id };
                await db.Carts.AddAsync(cart);
                await db.SaveChangesAsync();
            }
           
            return cart.CartId;
        }
    }
}
