using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.DiscountRepos
{
    public class DiscountRepo:GenericRepo<Discount,int>
    {
        public DiscountRepo(BlinkDbContext db):base(db){}
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
        public async Task<Discount?> GetRunningDiscountById(int id)
        {
            return await db.Discounts
                 .Include(d => d.ProductDiscounts)
                 .Where(d => d.DiscountId == id)
                 .Where(d => d.DiscountFromDate <= DateTime.UtcNow)
                 .Where(d => d.DiscountEndDate >= DateTime.UtcNow)
                 .Where(d => !d.IsDeleted)
                 .Where(d => d.ProductDiscounts.Any(pd => !pd.IsDeleted))
                 .OrderByDescending(d => d.DiscountFromDate)
                 .FirstOrDefaultAsync();
                 
        }
        public async Task<ICollection<Discount>> GetAllDiscounts()
        {
            var result= await db.Discounts
                .Include(pd=>pd.ProductDiscounts)
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(b=>b.Brand)
                .Include(pd=>pd.ProductDiscounts)
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(c=>c.Category)
                            .ThenInclude(pc=>pc.ParentCategory)
                .Include(pd=>pd.ProductDiscounts)
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(spi=>spi.StockProductInventories)
                            .Where(spi=>!spi.IsDeleted)
                .Where(d=>d.DiscountFromDate <= DateTime.UtcNow && d.DiscountEndDate >= DateTime.UtcNow && !d.IsDeleted)
                .ToListAsync();

            foreach (var discount in result)
            {
                discount.ProductDiscounts = discount.ProductDiscounts
                    .Where(pd => !pd.Product.IsDeleted)
                    .ToList();
            }
            return result;
        }
        public async Task<Discount?> GetDiscountById(int id)
        {
            var discount= await db.Discounts
               .Include(pd => pd.ProductDiscounts)
                   .ThenInclude(p => p.Product)
                       .ThenInclude(b => b.Brand)
               .Include(pd => pd.ProductDiscounts)
                   .ThenInclude(p => p.Product)
                       .ThenInclude(c => c.Category)
                           .ThenInclude(pc => pc.ParentCategory)
               .Include(pd => pd.ProductDiscounts)
                   .ThenInclude(p => p.Product)
                       .ThenInclude(spi => spi.StockProductInventories)
                           .Where(spi => !spi.IsDeleted)
               .Where(d => d.DiscountFromDate <= DateTime.UtcNow && d.DiscountEndDate >= DateTime.UtcNow && !d.IsDeleted)
               .FirstOrDefaultAsync();
            discount.ProductDiscounts= discount.ProductDiscounts.Where(pd=>!pd.Product.IsDeleted).ToList();
            return discount;
        }
        public async Task CreateDiscount(Discount discount)
        {
            await db.Discounts.AddAsync(discount);
            await SaveChanges();
        }
        public async Task UpdateDiscount(Discount discount)
        {
            var CurrentDiscount = await db.Discounts
                .Include(pd=>pd.ProductDiscounts)
                .FirstOrDefaultAsync(d=>d.DiscountId==discount.DiscountId);
            if(CurrentDiscount != null)
            {
                var prdDiscounts = CurrentDiscount.ProductDiscounts;
                db.ProductDiscounts.RemoveRange(prdDiscounts);
                await SaveChanges();
                CurrentDiscount.DiscountPercentage=discount.DiscountPercentage;
                CurrentDiscount.DiscountFromDate = discount.DiscountFromDate;
                CurrentDiscount.DiscountEndDate = discount.DiscountEndDate;
                CurrentDiscount.ProductDiscounts = discount.ProductDiscounts;
                await SaveChanges();
            }
        }
        public async Task DeleteDiscount(int id)
        {
            var CurrentDiscount = await db.Discounts.Include(pd => pd.ProductDiscounts).FirstOrDefaultAsync(d => d.DiscountId == id);
            if(CurrentDiscount != null)
            {
                foreach(ProductDiscount prdDiscount in CurrentDiscount.ProductDiscounts)
                {
                    prdDiscount.IsDeleted = true;
                }
                CurrentDiscount.IsDeleted = true;
                await SaveChanges();
            }
        }
    }
}
