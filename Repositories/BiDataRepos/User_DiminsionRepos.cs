using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class User_DiminsionRepos : GenericRepo<ApplicationUser, string>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public User_DiminsionRepos(BlinkDbContext _db)
            : base(_db)
        {
            _blinkDbContext = _db;
        }
        //public async override Task<List<ApplicationUser>> GetAll()
        //{
        //    return await _blinkDbContext.Users.ToListAsync();
        //}

        // for solve loading server for bi :
        public async IAsyncEnumerable<ApplicationUser> GetAllAsStream()
        {
            await foreach (var user in _blinkDbContext.Users.AsAsyncEnumerable())
            {
                yield return user;
            }
        }

    }
}
