using Blink_API.DTOs.BiDataDtos;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Branch_InventoryRepos: GenericRepo<Branch,int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Branch_InventoryRepos(BlinkDbContext context) : base(context)
        {
            _blinkDbContext = context;
        }
        public async IAsyncEnumerable<Branch> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.Branches
                .Where(b => b.IsDeleted == false)
                .AsAsyncEnumerable())
            {
                yield return item;
            }
        }

        #region old
        //public async override Task<List<Branch>> GetAll()
        //{
        //    return await _blinkDbContext.Branches
        //        .Where(b => b.IsDeleted == false)
        //        .ToListAsync();
        //}
        #endregion

    }

}
