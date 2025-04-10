using AutoMapper;
using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.BrandRepository
{
    public class BrandRepos : GenericRepo<Brand, int>
    {
        private readonly BlinkDbContext db;


        public BrandRepos(BlinkDbContext _db) : base(_db)
        {
            db = _db;

        }
        // display brands:
        public override async Task<List<Brand>> GetAll()
        {
            return await db.Brands
                .AsNoTracking()
                .Where(b => b.IsDeleted==false)
                .ToListAsync();
        }

        // get by id :
        public async override Task<Brand?> GetById(int id)
        {
            return await db.Brands
                .Where(b => b.BrandId == id && b.IsDeleted == false)
                 
                .FirstOrDefaultAsync();
        }

        // get by name :
        public async Task<List<Brand>> GetByName(string name)
        {
            return await db.Brands
                .Where(b => b.BrandName.Contains(name) && b.IsDeleted == false)
                .ToListAsync();
        }

        // insert brand
        public async Task<Brand> InsertBrand(Brand brand)
        {
            Add(brand);
            await SaveChanges();
            return brand;
        }
        // update Brand
        public async Task<Brand> UpdateBrand(int id,Brand brand)
        {
            Update(brand);
            await SaveChanges();
            return brand;
        }

        // soft delete brand
        public async Task SoftDeleteBrand(int id)
        {
            Delete(id);
            await SaveChanges();

        }
    }
}
