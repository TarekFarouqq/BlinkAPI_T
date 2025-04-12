using Blink_API.Models;
using Blink_API.Repositories;

using Blink_API.Repositories.BrandRepository;

using Blink_API.Repositories.BranchRepos;
 
using Blink_API.Repositories.CartRepos;
using Blink_API.Repositories.DiscountRepos;
using Blink_API.Repositories.InventoryRepos;
using StackExchange.Redis;
using Microsoft.EntityFrameworkCore;
using Blink_API.Repositories.Order;
using AutoMapper;

namespace Blink_API
{
    public class UnitOfWork
    {
        private readonly BlinkDbContext db;

        public DbContext Context => db;
        BrandRepos brandRepo;
        ProductRepo productRepo;
        CategoryRepo categoryRepo;
        DiscountRepo discountRepo;
        CartRepo cartRepo;
        CartDetailsRepo cartDetailsRepo;
        BranchRepos branchRepos;
        InventoryRepo inventoryRepo;
        orderRepo orderRepo;

        public UnitOfWork(BlinkDbContext _db)
        {
            db = _db;
        }

       

        public orderRepo OrderRepo
        {
            get
            {
                if (orderRepo == null)
                {
                    orderRepo = new orderRepo(db);
                }
                return orderRepo;
            }
        }

        public ProductRepo ProductRepo
        {
            get
            {
                if (productRepo == null)
                {
                    productRepo = new ProductRepo(db);
                }
                return productRepo;
            }
        }

        public InventoryRepo InventoryRepo
        {
            get
            {
                if (inventoryRepo == null)
                {
                    inventoryRepo = new InventoryRepo(db);
                }
                return inventoryRepo;
            }
        }

        public CategoryRepo CategoryRepo
        {
            get
            {
                if (categoryRepo == null)
                {
                    categoryRepo = new CategoryRepo(db);
                }
                return categoryRepo;
            }
        }

        public DiscountRepo DiscountRepo
        {
            get
            {
                if (discountRepo == null)
                {
                    discountRepo = new DiscountRepo(db);
                }
                return discountRepo;
            }
        }
       

        public CartRepo CartRepo
        {
            get
            {
                if (cartRepo == null)
                {
                    cartRepo = new CartRepo(db);
                }
                return cartRepo;
            }
        }

        public CartDetailsRepo CartDetailsRepo
        {
            get
            {
                if (cartDetailsRepo == null)
                {
                    cartDetailsRepo = new CartDetailsRepo(db);
                }
                return cartDetailsRepo;
            }
        }


        public BrandRepos BrandRepos
        {
            get
            {
                if (brandRepo == null)
                {
                    brandRepo = new BrandRepos(db);
                }
                return brandRepo;
            }
        }

        
 
        public BranchRepos BranchRepos
        {
            get
            {
                if (branchRepos == null)
                {
                    branchRepos = new BranchRepos(db);
                }
                return branchRepos;
            }
        }

        public async Task<int> CompleteAsync()
         => await db.SaveChangesAsync();

    }
}
