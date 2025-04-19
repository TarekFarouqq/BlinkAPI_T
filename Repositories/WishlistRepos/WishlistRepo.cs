using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.WishlistRepos
{
    public class WishlistRepo : GenericRepo<WishList, int>
    {
        private readonly BlinkDbContext db;


        public WishlistRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }


        public async Task<List<WishList>> GetAll(int pgNumber, int pgSize)
        {
            return await db.WishLists
                        .AsNoTracking()
                        .Skip((pgNumber - 1) * pgSize)
                        .Take(pgSize)
                .Include(c => c.WishListDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                    .ThenInclude(p=>p.ProductImages)
                .Where(p => !p.IsDeleted)
                .ToListAsync();

        }

        public async Task<WishList?> GetByUserId(string id)
        {
            return await db.WishLists
                        .AsNoTracking()
                .Include(c => c.WishListDetails.Where(cd => !cd.IsDeleted))
                    .ThenInclude(c => c.Product)
                    .ThenInclude(p => p.ProductImages)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.UserId == id);
        }

        public async Task<int?> AddWishList(string id)
        {

            var wishList = await GetByUserId(id);

            if (wishList == null)
            {
                wishList = new WishList() { UserId = id };
                await db.WishLists.AddAsync(wishList);
                await db.SaveChangesAsync();
            }

            return wishList.WishlistId;
        }


        public async Task<bool> ClearWishList(int id)
        {
            var wishList = await db.WishLists
                .Include(p => p.WishListDetails)
                .FirstOrDefaultAsync(c => c.WishlistId == id && !c.IsDeleted);

            if (wishList == null)
                return false;

            bool hasWishListDetails = wishList.WishListDetails.Any(p => !p.IsDeleted);

            if (hasWishListDetails)
            {
                foreach (var wishListDetail in wishList.WishListDetails)
                {
                    wishListDetail.IsDeleted = true;
                }
            }

            await db.SaveChangesAsync();

            return true;
        }
    }
}
