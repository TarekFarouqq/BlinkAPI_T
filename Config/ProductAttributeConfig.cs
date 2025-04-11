using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class ProductAttributeConfig : IEntityTypeConfiguration<ProductAttributes>
    {
        public void Configure(EntityTypeBuilder<ProductAttributes> builder)
        {
            builder.HasKey(pa => new { pa.ProductId, pa.AttributeId, pa.AttributeValue });
        }
    }

}
