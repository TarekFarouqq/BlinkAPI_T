using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Order
{
    public class OrderDetailsRepository:GenericRepo<OrderDetail,int>
    {
       

        public OrderDetailsRepository(BlinkDbContext db)
            : base(db)
        {
           
        }

        public async Task<List<OrderDetail>> GetDetailsByOrderId(int orderId)
        {
            return await db.OrderDetails
               .Include(od => od.product)
               .ThenInclude(p => p.StockProductInventories)
               .Where(od => od.OrderHeaderId == orderId && !od.IsDeleted)
               .ToListAsync();
        }

    }
    }

