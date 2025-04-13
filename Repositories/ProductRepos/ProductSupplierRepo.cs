using Blink_API.Models;

namespace Blink_API.Repositories.ProductRepos
{
    public class ProductSupplierRepo: GenericRepo<ReviewSuppliedProduct, int>
    {
        private BlinkDbContext db;
        public ProductSupplierRepo(BlinkDbContext _db):base(_db)
        {
            db = _db;
        }
    }
}
