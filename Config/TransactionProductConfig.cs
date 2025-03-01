using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class TransactionProductConfig : IEntityTypeConfiguration<TransactionProduct>
    {
        public void Configure(EntityTypeBuilder<TransactionProduct> builder)
        {
            builder.HasKey(s => new { s.ProductId, s.InventoryTransactionId });
        }
    }
}
