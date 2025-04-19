using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.WishlistRepos
{
    public class WishListDetailsRepo : GenericRepo<WishListDetail, int>
    {
        private readonly BlinkDbContext db;
        public WishListDetailsRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }
        public async Task<WishListDetail?> GetById(int wishlistId, int productId)
        {
            return await db.WishListDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.WishListId == wishlistId && p.ProductId == productId);
        }
    }
}
