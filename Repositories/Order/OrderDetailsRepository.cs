using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Order
{
    public class OrderDetailsRepository:GenericRepo<OrderDetail,int>
    {
        private readonly BlinkDbContext _blinkDbContext;

        public OrderDetailsRepository(BlinkDbContext blinkDbContext):base(blinkDbContext)
        {
            _blinkDbContext = blinkDbContext;
        }
        public async Task<List<OrderDetail>> GetDetailsByOrderId(int orderId)
        {
            return await _blinkDbContext.OrderDetails
               .Include(od => od.product)
               .ThenInclude(p => p.StockProductInventories)
               .Where(od => od.OrderHeaderId == orderId && !od.IsDeleted)
               .ToListAsync();
        }

    }
    }

