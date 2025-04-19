using Blink_API.Models;
using Microsoft.AspNetCore.Http.HttpResults;
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


        public async Task<WishListDetail?> DeleteWishListDetail(int productId, int wishListId)
        {
            var wishListDetail = await db.WishListDetail
                .AsNoTracking()
                .FirstOrDefaultAsync(p => p.WishListId == wishListId && p.ProductId == productId);

            if (wishListDetail == null)
            {
                return null;
            }
            else
            {
                wishListDetail.IsDeleted = true;
                db.WishListDetail.Update(wishListDetail);
                await db.SaveChangesAsync();
            }

            return wishListDetail;
        }




    }
}
