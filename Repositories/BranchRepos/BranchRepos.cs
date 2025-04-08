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

        public async override Task<List<Branch>> GetAll()
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


        public async Task<bool> Update(int id, Branch updatedBranch)
        {
            var existingBranch = await _blinkDbContext.Branches.FindAsync(id);
            if (existingBranch == null || existingBranch.IsDeleted)
                return false;

            existingBranch.BranchName = updatedBranch.BranchName;
            existingBranch.BranchAddress = updatedBranch.BranchAddress;
            existingBranch.Phone = updatedBranch.Phone;

            _blinkDbContext.Branches.Update(existingBranch);
            await _blinkDbContext.SaveChangesAsync();
            return true;
        }
        
        
        //public override async Task<bool> Delete(int id)
        //{
        //    var branch = await _blinkDbContext.Branches.Include(b => b.Inventories).FirstOrDefaultAsync(b => b.BranchId == id);

        //    if (branch == null || branch.IsDeleted)
        //        return false;

        //    if (branch.Inventories.Any())
        //        return false; 

        //    return await base.Delete(id);
        //}




    }
}
