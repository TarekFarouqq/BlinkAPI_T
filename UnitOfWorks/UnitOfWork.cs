using Blink_API.Models;
using Blink_API.Repositories;

namespace Blink_API.UnitOfWorks
{
    public class UnitOfWork
    {
        private readonly BlinkDbContext db;
        ProductRepo productRepo;

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

    }
}
