using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class User_DiminsionRepos : GenericRepo<ApplicationUser, string>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public User_DiminsionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
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
