using Blink_API.Models;
using Blink_API.Repositories;

namespace Blink_API
{
    public class UnitOfWork
    {
        private readonly BlinkDbContext db;
        ProductRepo productRepo;
        CategoryRepo categoryRepo;
        public UnitOfWork(BlinkDbContext _db)
        {
            db = _db;
        }
        public ProductRepo ProductRepo 
        {
            get 
            {
                if (productRepo == null) 
                {
                    productRepo = new ProductRepo (db);
                }
                return productRepo;
             }
        }
        public CategoryRepo CategoryRepo
        {
           get
            {
               if(categoryRepo == null)
                {
                    categoryRepo = new CategoryRepo (db);
                }
                return categoryRepo;
            }
        }

    }
}
