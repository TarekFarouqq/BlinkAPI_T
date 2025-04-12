using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Order
{
    public class orderRepo:GenericRepo<OrderHeader,int>
    {
        private readonly BlinkDbContext _db;

        public orderRepo(BlinkDbContext db) : base(db)
        {
            _db = db;
        }

        public async Task<List<OrderHeader>> GetOrdersWithDetails()
        {
            return await _db.OrderHeaders
                .Include(o => o.OrderDetails)
                .Include(o => o.Payment)
                .Include(o => o.Cart)
                .ToListAsync();


        }
        public async Task<OrderHeader?> GetOrderByIdWithDetails(int id)
        {
            return await _db.OrderHeaders
                .Include(o => o.OrderDetails)
                .Include(o => o.Payment)
                .Include(o => o.Cart)
                .FirstOrDefaultAsync(o => o.OrderHeaderId == id);
        }

    }
}
