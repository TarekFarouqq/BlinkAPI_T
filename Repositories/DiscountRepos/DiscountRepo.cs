using System.Runtime.Intrinsics.Arm;
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
            var result =  await db.Discounts
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                .Include(pd=>pd.ProductDiscounts)
                    .ThenInclude(p=>p.Product)
                        .ThenInclude(b=>b.Brand)
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(b => b.Category)
                            .ThenInclude(pc=>pc.ParentCategory)
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(b => b.StockProductInventories)
                .Where(pd=>pd.ProductDiscounts.Count> 0)
                .ToListAsync();
            foreach(var discount in result)
            {
                discount.ProductDiscounts = discount.ProductDiscounts.Where(p => !p.Product.IsDeleted).ToList();
            }
            result=result.Where(d=>!d.IsDeleted).ToList();
            return result;
        }
        public async Task<Discount?> GetDiscountById(int id)
        {
            var result = await db.Discounts
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(b => b.Brand)
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(b => b.Category)
                            .ThenInclude(pc => pc.ParentCategory)
                .Include(pd => pd.ProductDiscounts)
                    .ThenInclude(p => p.Product)
                        .ThenInclude(b => b.StockProductInventories)
                .Where(pd => pd.ProductDiscounts.Count > 0)
                .FirstOrDefaultAsync(d=>d.DiscountId==id && !d.IsDeleted);

            result.ProductDiscounts = result.ProductDiscounts.Where(p => !p.Product.IsDeleted).ToList();
            return result;
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
        public async Task<List<Discount>> GetDiscountBetween2Dates(DateTime startDate,DateTime endDate)
        {
            return await db.Discounts
                 .Include(d => d.ProductDiscounts)
                 .Where(d => d.DiscountFromDate >= startDate)
                 .Where(d => d.DiscountEndDate <= endDate)
                 .Where(d => !d.IsDeleted)
                 .Where(d => d.ProductDiscounts.Any(pd => !pd.IsDeleted))
                 .OrderByDescending(d => d.DiscountFromDate)
                 .ToListAsync();
        }
    }
}
