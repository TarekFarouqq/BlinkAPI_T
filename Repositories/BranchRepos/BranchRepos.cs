using Blink_API.DTOs.BranchDto;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BranchRepos
{
    public class BranchRepos : GenericRepo<Branch,int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public BranchRepos(BlinkDbContext blinkDbContext ):base(blinkDbContext) 
        {
            _blinkDbContext = blinkDbContext;
        }

        public async override Task<List<Branch>> GetAllAsync()
        {
            return await _blinkDbContext.Branches.Where(b => b.IsDeleted==false).Include(b => b.Inventories).ToListAsync();
        }

        public async override Task<Branch?> GetById(int id)
        {
            return await _blinkDbContext.Branches
                .Where(b => b.BranchId == id && b.IsDeleted == false)
                .Include(b => b.Inventories) 
                .FirstOrDefaultAsync();
        }


        public async Task<Branch?> GetBranchByIdAsync(int branchId)
        {
            return await _blinkDbContext.Set<Branch>().FindAsync(branchId); 
        }

    }
}
