using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class OrderHeaderConfig : IEntityTypeConfiguration<OrderHeader>
    {
        public void Configure(EntityTypeBuilder<OrderHeader> builder)
        {
            builder.Property(pd => pd.OrderSubtotal).HasPrecision(18, 4);
            builder.Property(pd => pd.OrderTax).HasPrecision(18, 4);
            builder.Property(pd => pd.OrderShippingCost).HasPrecision(18, 4);
            builder.Property(pd => pd.OrderTotalAmount).HasPrecision(18, 4);
        }
    }

}
