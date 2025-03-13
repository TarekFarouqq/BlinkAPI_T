using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class WishListDetailConfig : IEntityTypeConfiguration<WishListDetail>
    {
        public void Configure(EntityTypeBuilder<WishListDetail> builder)
        {
            builder.HasKey(s => new { s.ProductId, s.WishListId });

            builder.HasOne(wld => wld.Product)
                .WithMany(p => p.WishListDetails)
                .HasForeignKey(wld => wld.ProductId)
                .OnDelete(DeleteBehavior.NoAction); // Changed to NoAction

            // Relationship with Cart
            builder.HasOne(wld => wld.WishList)
                .WithMany(c => c.WishListDetails)
                .HasForeignKey(wld => wld.WishListId)
                .OnDelete(DeleteBehavior.Cascade);
        }



    }
}
