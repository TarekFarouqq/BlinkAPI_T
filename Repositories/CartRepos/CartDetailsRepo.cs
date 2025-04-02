using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.CartRepos
{
    public class CartDetailsRepo : GenericRepo<CartDetail, int>
    {
        private readonly BlinkDbContext db;
        public CartDetailsRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }
         public async Task<CartDetail?> GetById(int cartId, int productId)
        {
            return await db.CartDetails
                .AsNoTracking()
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.CartId == cartId && p.ProductId == productId);
        }
    }
}
