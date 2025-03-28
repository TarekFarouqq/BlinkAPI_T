using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class ReviewComment
    {
        // id is not identity !!!!!!!

        public int CommentId { get; set; }
        [Required]
        public int ReviewId { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "Comment content cannot exceed 500 characters.")]
        public string Content { get; set; }
        [ForeignKey("ReviewId")]
        public virtual Review Review { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}
