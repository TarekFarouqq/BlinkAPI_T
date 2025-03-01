using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Review
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ReviewId { get; set; }
        [Range(1, 5, ErrorMessage = "Rate must be between 1 and 5.")]
        public int Rate { get; set; }
        public DateTime CreationDate { get; set; }
        [Required]
        public string UserId { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }
        [ForeignKey("UserId")]

        public virtual ApplicationUser User { get; set; }
        public virtual ICollection<ReviewComment> ReviewComments { get; set; } = new HashSet<ReviewComment>();
        public bool IsDeleted { get; set; } = false;


    }
}
