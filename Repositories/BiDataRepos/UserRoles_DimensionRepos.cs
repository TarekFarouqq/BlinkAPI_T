using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class UserRoles_DimensionRepos : GenericRepo<IdentityUserRole<string>, int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public UserRoles_DimensionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
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
