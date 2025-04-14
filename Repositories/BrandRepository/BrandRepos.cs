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
        public override async Task<List<Brand>> GetAll()
        {
            return await db.Brands
                .AsNoTracking()
                .Where(b => !b.IsDeleted)
                .ToListAsync();
        }
        public async override Task<Brand?> GetById(int id)
        {
            return await db.Brands
                .Where(b => b.BrandId == id && !b.IsDeleted)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Brand>> GetByName(string name)
        {
            return await db.Brands
                .Where(b => b.BrandName.Contains(name) && b.IsDeleted == false)
                .ToListAsync();
        }

 

        // insert brand

 

        public async Task<Brand> InsertBrand(Brand brand)
        {
            db.Brands.Add(brand);
            await SaveChanges();
            return brand;
        }
        public async Task<Brand> UpdateBrand(int id, Brand brand)
        {
            var oldBrand = await GetById(id);
            if (oldBrand != null)
            {
                oldBrand.BrandName = brand.BrandName;
                oldBrand.BrandImage = brand.BrandImage;
                oldBrand.BrandDescription = brand.BrandDescription;
                oldBrand.BrandWebSiteURL = brand.BrandWebSiteURL;
                await SaveChanges();
            }
            return brand;
        }
        public async Task SoftDeleteBrand(int id)
        {
            var brand = await GetById(id);
            if (brand != null)
            {
                brand.IsDeleted = true;
            }
            await SaveChanges();
        }
    }
}