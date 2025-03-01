using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class TransactionDetailConfig : IEntityTypeConfiguration<TransactionDetail>
    {
        public void Configure(EntityTypeBuilder<TransactionDetail> builder)
        {
            builder.HasKey(s => new { s.UserId, s.InventoryTransactionHeaderId });

           
            builder.HasOne(td => td.SrcInventory)
                .WithMany(i => i.SentTransactions)
                .HasForeignKey(td => td.SrcInventoryId)
                .OnDelete(DeleteBehavior.NoAction); 

            builder.HasOne(td => td.DistInventory)
                .WithMany(i => i.ReceivedTransactions)
                .HasForeignKey(td => td.DistInventoryId)
                .OnDelete(DeleteBehavior.NoAction); 

        }
    }
}
