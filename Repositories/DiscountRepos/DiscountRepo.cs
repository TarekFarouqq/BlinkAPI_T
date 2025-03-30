using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.DiscountRepos
{
    public class DiscountRepo:GenericRepo<Discount,int>
    {
        private readonly BlinkDbContext db;
        public DiscountRepo(BlinkDbContext _db):base(_db)
        {
            db=_db;
        }
        public async Task<ICollection<Discount>> GetRunningDiscounts()
        {
            return await db.Discounts
                 .Include(d => d.ProductDiscounts)
                 .Where(d => d.DiscountFromDate <= DateTime.UtcNow)
                 .Where(d => d.DiscountEndDate >= DateTime.UtcNow)
                 .Where(d => !d.IsDeleted)
                 .Where(d => d.ProductDiscounts.Any(pd => !pd.IsDeleted))
                 .OrderByDescending(d=>d.DiscountFromDate)
                 .ToListAsync();
        }
    }
}
