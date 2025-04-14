using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BiDataRepos
{
    public class Role_DiminsionRepos:GenericRepo<IdentityRole, string>
    {
        private readonly BlinkDbContext _blinkDbContext;
        public Role_DiminsionRepos(BlinkDbContext blinkDbContext) : base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }

        public async IAsyncEnumerable<IdentityRole> GetAllAsStream()
        {
            await foreach (var role in _blinkDbContext.Roles.AsAsyncEnumerable())
            {
                yield return role;
            }
        }

        #region old 
        //public async override Task<List<IdentityRole>> GetAll()
        //{
        //    return await _blinkDbContext.Roles.ToListAsync();
        //}
        #endregion
    }
}
