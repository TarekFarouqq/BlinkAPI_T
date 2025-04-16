using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Order
{
    public class OrderHeaderRepository:GenericRepo<OrderHeader,int>
    {
        private readonly BlinkDbContext _db;

        public OrderHeaderRepository(BlinkDbContext db) : base(db)
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
        public async Task<OrderHeader?> GetOrderByIdWithDetails(int orderId)
        {
            return await _db.OrderHeaders
               
                .Include(o => o.OrderDetails.Where(od => !od.IsDeleted))
                    .ThenInclude(od => od.product) 
                .Include(o => o.Payment) 
                .Include(o => o.Cart)
                .Where(o => !o.IsDeleted && o.OrderHeaderId == orderId)
                 .FirstOrDefaultAsync();
        }

        public async Task<OrderHeader?> GetOrderByPaymentIntentId(string paymentIntentId)
        {
            return await _db.OrderHeaders
                .Include(o => o.OrderDetails)
                .ThenInclude(p=>p.product)
                 .ThenInclude(p => p.ProductImages)
                .Include(o => o.Payment)
                .Include(o => o.Cart)
                .FirstOrDefaultAsync(o => o.PaymentIntentId == paymentIntentId);
        }

    


    }
}
