using System.Linq.Expressions;
using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BranchRepos
{
    public class BranchRepos : GenericRepo<Branch,int>
    {
        

        public BranchRepos(BlinkDbContext db)
            : base(db)
        {
            
        }

        public async Task<List<Branch>> GetAll(int pgNumber,int pgSize)
        {
            return await db.Branches
                .Where(b => b.IsDeleted==false)
                .Skip((pgNumber - 1) * pgSize)  
                .Take(pgSize)
                .Include(b => b.Inventories)
                .ToListAsync();
        }

        public async override Task<Branch?> GetById(int id)
        {
            return await db.Branches
                .Where(b => b.BranchId == id && b.IsDeleted == false)
                .Include(b => b.Inventories) 
                .FirstOrDefaultAsync();
        }


        public async Task<bool> Update(int id, Branch updatedBranch)
        {
            var existingBranch = await db.Branches.FindAsync(id);
            if (existingBranch == null || existingBranch.IsDeleted)
                return false;

            existingBranch.BranchName = updatedBranch.BranchName;
            existingBranch.BranchAddress = updatedBranch.BranchAddress;
            existingBranch.Phone = updatedBranch.Phone;

            db.Branches.Update(existingBranch);
            await db.SaveChangesAsync();
            return true;
        }

        public async Task<Branch?> GetFirstOrDefaultAsync(Expression<Func<Branch, bool>> predicate)
        {
            return await db.Set<Branch>().FirstOrDefaultAsync(predicate);
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
