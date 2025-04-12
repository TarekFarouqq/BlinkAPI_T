using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Discount_DiminsionRepo : GenericRepo<Discount, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Discount_DiminsionRepo(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async override Task<List<Discount>> GetAll()
        {
            return await _blinkDbContext.Discounts
                .Where(b => b.IsDeleted == false)
                .ToListAsync();
        }
    }
}
