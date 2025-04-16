using AutoMapper;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.UserRepos
{
    public class UserRepos : GenericRepo<ApplicationUser, string>
    {
        private readonly BlinkDbContext db;
       
        public UserRepos(BlinkDbContext _db) : base(_db)
        {
            db = _db;
            
        }
        // Get All Active Users :
        public override async Task<List<ApplicationUser>> GetAll()
        {
            return await db.Users
                .AsNoTracking()
                .Where(u => !u.IsDeleted)
                .ToListAsync();
        }
        // Get User By Id :
        public async override Task<ApplicationUser?> GetById(string id)
        {
            return await db.Users
                .Where(u => u.Id == id && !u.IsDeleted)
                .FirstOrDefaultAsync();
        }
        // Get Users by Name :
        public async Task<List<ApplicationUser>> GetByUserName(string name)
        {
            return await db.Users
                .Where(u => u.UserName.Contains(name) && u.IsDeleted == false)
                .ToListAsync();
        }

        // insert user:
        public async Task<ApplicationUser> InsertUser(ApplicationUser user)
        {
            db.Users.Add(user);
            await SaveChanges();
            return user;
        }
        // update user:
        public async Task<ApplicationUser> UpdateUser(string id, ApplicationUser user)
        {
            var oldUser = await GetById(id);
            if (oldUser != null)
            {
                oldUser.FirstName = user.FirstName;
                oldUser.LastName = user.LastName;
                oldUser.Email = user.Email;
                oldUser.PhoneNumber = user.PhoneNumber;
                oldUser.Address = user.Address;
                await SaveChanges();
            }
            return user;
        }

        // soft delete user:
        public async Task SoftDeleteUser(string id)
        {
            var user = await GetById(id);
            if (user != null)
            {
                user.IsDeleted = true;
                await SaveChanges();
            }
        }

        public async Task<List<ApplicationUser>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Users
                .Where(u => !u.IsDeleted)
                .OrderBy(u => u.UserName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToListAsync();
        }

        // Get number of pages
        public async Task<int> GetPagesCount(int pageSize)
        {
            var count = await db.Users.CountAsync(u => !u.IsDeleted);
            return (int)Math.Ceiling(count / (double)pageSize);
        }


        public async Task<ApplicationUser> UpdateUserAddress(string id, string newAddress)
        {
            var user = await GetById(id);
            if (user != null) 
            user.Address = newAddress;
            db.Users.Update(user);
            await SaveChanges();
            return user;

        }

        public async Task<ApplicationUser> UpdateUserPhoneNumber(string id, string newPhone)
        {
            var user = await GetById(id);
            if (user != null)
                user.PhoneNumber = newPhone;
            db.Users.Update(user);
            await SaveChanges();
            return user;
        }

    }
}
