using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class UserRoles_DimensionRepos : GenericRepo<IdentityUserRole<string>, int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public UserRoles_DimensionRepos(BlinkDbContext _db)
            : base(_db)
        {
            _blinkDbContext = _db;
        }

        public async IAsyncEnumerable<IdentityUserRole<string>> GetAllAsStream()
        {
            await foreach (var userRole in _blinkDbContext.UserRoles.AsAsyncEnumerable())
            {
                yield return userRole;
            }
        }

        #region old 
        //public async override Task<List<IdentityUserRole<string>>> GetAll()
        //{
        //    return await _blinkDbContext.UserRoles.ToListAsync();
        //}
        #endregion
    }
}
