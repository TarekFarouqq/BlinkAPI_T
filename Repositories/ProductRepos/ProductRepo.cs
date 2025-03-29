using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories
{
    public class ProductRepo : GenericRepo<Product, int>
    {
        private readonly BlinkDbContext db;
        public ProductRepo(BlinkDbContext _db) : base(_db)
        {
            db = _db;
        }
        public override async Task<List<Blink_API.Models.Product>> GetAll()
        {
            return await db.Products
                .Include(u=>u.User)
                .Include(b=>b.Brand)
                .Include(c=>c.Category)
                .Include(i=>i.ProductImages)
                .Include(r=>r.Reviews)
                .ThenInclude(rc=>rc.ReviewComments)
                .Where(p=>!p.IsDeleted)
                .ToListAsync(); 
        }
        public override async Task<Blink_API.Models.Product?> GetById(int id)
        {
            return await db.Products
                .Include(u => u.User)
                .Include(b => b.Brand)
                .Include(c => c.Category)
                .Include(i => i.ProductImages)
                .Include(r => r.Reviews)
                .ThenInclude(rc => rc.ReviewComments)
                .Where(p => !p.IsDeleted)
                .FirstOrDefaultAsync(p => p.ProductId == id);
        }
    }
}
