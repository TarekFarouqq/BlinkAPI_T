using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class paymentDiminsionRepos : GenericRepo<Payment,int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public paymentDiminsionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async override Task<List<Payment>> GetAll()
        {
            return await _blinkDbContext.Payments
                 
                .Where(b => b.IsDeleted == false)
                .ToListAsync();
        }
    }
}
