using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class paymentDiminsionRepos : GenericRepo<Blink_API.Models.Payment, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public paymentDiminsionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        //public async override Task<List<Blink_API.Models.Payment>> GetAll();


        //public async IAsyncEnumerable<Blink_API.Models.Payment> GetAllAsStream()

        //{
        //    await foreach (var payment in _blinkDbContext.Payments
        //        .Where(b => b.IsDeleted == false)
        //        .AsAsyncEnumerable())
        //    {
        //        yield return payment;
        //    }
        //}
        #region old
        //public async override Task<List<Blink_API.Models.Payment>> GetAll()
        //{
        //    return await _blinkDbContext.Payments
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}

        public override  IAsyncEnumerable<Blink_API.Models.Payment> GetAllAsStream()
{
            return _blinkDbContext.Payments
                        .AsNoTracking()
       // .Where(p => p.IsDeleted == false)
        .AsAsyncEnumerable();
}

        #endregion
    }
}
