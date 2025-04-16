using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace Blink_API.DTOs.ProductDTOs
{
    public class UserReviewCommentDTO
    {
        public string UserId { get; set; }
        public int ProductId { get; set; }
        public int ReviewRate { get; set; }
        public string Comment { get; set; }
    }
}
