using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class ProductDiscountConfig : IEntityTypeConfiguration<ProductDiscount>
    {
        public void Configure(EntityTypeBuilder<ProductDiscount> builder)
        {
            builder.HasKey(s => new { s.ProductId, s.DiscountId });
            builder.Property(pd => pd.DiscountAmount).HasPrecision(18, 4);
        }
    }
}
