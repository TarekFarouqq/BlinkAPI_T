using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Review_DimensionRepos:GenericRepo<Review,int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Review_DimensionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async IAsyncEnumerable<Review> GetAllAsStream()
        {
            await foreach (var review in _blinkDbContext.Reviews
                .Include(b => b.User)
                .Include(b => b.Product)
                .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return review;
            }
        }

        //public async override Task<List<Review>> GetAll()
        //{
        //    return await _blinkDbContext.Reviews
        //        .Include(b => b.User)
        //        .Include(b => b.Product)
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}

    }

}
