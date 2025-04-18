using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class ReviewProductImagesConfig : IEntityTypeConfiguration<ReviewSuppliedProductImages>
    {
        public void Configure(EntityTypeBuilder<ReviewSuppliedProductImages> builder)
        {
            builder.HasKey(rpi => new { rpi.RequestId , rpi.ImagePath });
        }
    }
}
