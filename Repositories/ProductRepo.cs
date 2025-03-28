using Blink_API.Models;

namespace Blink_API.Repositories
{
    public class ProductRepo : GenericRepo<Product, int>
    {
        public ProductRepo(BlinkDbContext _db) : base(_db)
        {

        }
    }
}
