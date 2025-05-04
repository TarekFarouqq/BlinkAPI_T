using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Order
{
    public class OrderHeaderRepository:GenericRepo<OrderHeader,int>
    {
       
        public OrderHeaderRepository(BlinkDbContext db)
            : base(db)
        {
           
        }

        public async Task<List<OrderHeader>> GetOrdersByUserIdAsync(string userId)
        {
            var order = await db.OrderHeaders.Include(o => o.OrderDetails.Where(od => !od.IsDeleted))
                    .ThenInclude(od => od.product)
                    .ThenInclude(od => od.ProductImages)
                //.Include(o => o.Payment)
                .Include(o => o.Cart)
                .Where(o => !o.IsDeleted && o.Cart.UserId == userId)
                 .ToListAsync();

            return order;
        }

        public async Task<List<OrderHeader>> GetOrdersWithDetails()
        {
            return await db.OrderHeaders
                .Include(o => o.OrderDetails)
                .Include(o => o.Payment)
                .Include(o => o.Cart)
                .ToListAsync();


        }
        public async Task<OrderHeader?> GetOrderByIdWithDetails(int orderId)
        {
            var order = await db.OrderHeaders.Include(o => o.OrderDetails.Where(od => !od.IsDeleted))
                    .ThenInclude(od => od.product) 
                    .ThenInclude(od=>od.ProductImages)
                //.Include(o => o.Payment) 
                .Include(o => o.Cart)
                .Where(o => !o.IsDeleted && o.OrderHeaderId == orderId)
                 .FirstOrDefaultAsync();

            return order;
        }

        public async Task<OrderHeader?> GetOrderByPaymentIntentId(string paymentIntentId)
        {
            return await db.OrderHeaders
                .Include(o => o.OrderDetails)
                    .ThenInclude(p => p.product)
                        .ThenInclude(p => p.ProductImages)
                .Include(o => o.Payment)
                .Include(o => o.Cart)
                .Where(o=>o.Payment.PaymentIntentId==paymentIntentId)
                .FirstOrDefaultAsync();
        }


    }
}
