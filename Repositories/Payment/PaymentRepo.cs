using Blink_API.Models;
using Microsoft.EntityFrameworkCore;

namespace Blink_API.Repositories.Payment
{
    public class PaymentRepository : GenericRepo<Blink_API.Models.Payment, int>
    {
        private readonly BlinkDbContext _context;

        public PaymentRepository(BlinkDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<Blink_API.Models.Payment?> GetByPaymentIntentId(string paymentIntentId)
        {
            return await _context.Payments
                .FirstOrDefaultAsync(p => p.PaymentIntentId == paymentIntentId && !p.IsDeleted);
        }

    }
}
