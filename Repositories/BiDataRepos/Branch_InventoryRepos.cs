using Blink_API.DTOs.BiDataDtos;
using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Branch_InventoryRepos: GenericRepo<Inventory, int>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Branch_InventoryRepos(BlinkDbContext _db)
            : base(_db)
        {
            _blinkDbContext = _db;
        }
        public async IAsyncEnumerable<Inventory> GetAllAsStream()
        {
            await foreach (var item in _blinkDbContext.Inventories
                 .Include(i => i.Branch)
                // .Where(b => b.IsDeleted == false)
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
