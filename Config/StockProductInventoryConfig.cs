using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class StockProductInventoryConfig : IEntityTypeConfiguration<StockProductInventory>
    {
        public void Configure(EntityTypeBuilder<StockProductInventory> builder)
        {
            builder.HasKey(s => new { s.ProductId, s.InventoryId });
            builder.Property(pd => pd.StockUnitPrice).HasPrecision(18, 4);
        }
    }
}
