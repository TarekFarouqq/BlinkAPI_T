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
                .Include(b=>b.Products)
                .ToListAsync();
        }
        public async override Task<Brand?> GetById(int id)
        {
            return await db.Brands
                .Where(b => b.BrandId == id && !b.IsDeleted)
                  .Include(b => b.Products)
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
        public async Task<bool> SoftDeleteBrand(int id)
        {
            bool canDelete = true;
            var brand = await db.Brands
                .Include(b => b.Products)
                .FirstOrDefaultAsync(b => b.BrandId == id);
            if (brand != null)
            {
                var products = brand.Products.ToList();
                foreach (var product in products)
                {
                    if (product.IsDeleted == false)
                    {
                        canDelete = false;
                    }
                }
                if (canDelete)
                {
                    brand.IsDeleted = true;
                    await SaveChanges();
                }
            }
            return canDelete;
        
            
            //var brand = await GetById(id);
            //if (brand != null)
            //{
            //    brand.IsDeleted = true;
            //}
            //await SaveChanges();
        }
        //public async Task<bool> SoftDeleteBrand(int id)
        //{
        //    bool canDelete = true;
        //    var brand = await db.Brands
        //        .Include(b => b.Products)
        //        .FirstOrDefaultAsync(b => b.BrandId == id);
        //    if (brand != null)
        //    {
        //        var products = brand.Products.ToList();
        //        foreach (var product in products)
        //        {
        //            if (product.IsDeleted == false)
        //            {
        //                canDelete = false;
        //            }
        //        }
        //        if (canDelete)
        //        {
        //            brand.IsDeleted = true;
        //            await SaveChanges();
        //        }
        //    }
        //    return canDelete;
        //}


        public async Task<List<Brand>> GetAllPaginated(int pageNumber, int pageSize)
        {
            return await db.Brands
                .Where(b => !b.IsDeleted)
                .OrderBy(b => b.BrandName)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .Include(b => b.Products)
                .ToListAsync();
        }

        public async Task<int> GetPagesCount(int pageSize)
        {
            var count = await db.Brands.CountAsync(b => !b.IsDeleted);
            return (int)Math.Ceiling(count / (double)pageSize);
        }


    }
}