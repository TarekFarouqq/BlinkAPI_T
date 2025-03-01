using Blink_API.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Blink_API.Config
{
    public class ReviewCommentConfig : IEntityTypeConfiguration<ReviewComment>
    {
        public void Configure(EntityTypeBuilder<ReviewComment> builder)
        {
            builder.HasKey(s => new { s.CommentId, s.ReviewId });
        }
    }
}
