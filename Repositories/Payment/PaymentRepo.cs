using Blink_API.Hubs;
using Blink_API.Models;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Payment
{
    public class PaymentRepository : GenericRepo<Blink_API.Models.Payment, int>
    {
        

        public PaymentRepository(BlinkDbContext db)
            : base(db)
        {

        }

        public async Task<Blink_API.Models.Payment?> GetPaymentByIntentId(string paymentIntentId)
        {
            return await db.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId && !p.IsDeleted);
        }

    }
}
