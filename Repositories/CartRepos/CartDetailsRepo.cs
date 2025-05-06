using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.CartRepos
{
    public class CartDetailsRepo : GenericRepo<CartDetail, int>
    {
      
        public CartDetailsRepo(BlinkDbContext db)
            : base(db)
        {
        }
        public async Task<CartDetail?> GetById(int cartId, int productId)
        {
            return await db.CartDetails
                .AsNoTracking()
                .Include(p=>p.Product)
                .ThenInclude(pd=>pd.ProductDiscounts)
                .ThenInclude(d=>d.Discount)
                .FirstOrDefaultAsync(p => p.CartId == cartId && p.ProductId == productId);
        }
    }
}
