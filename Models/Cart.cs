using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Cart
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CartId { get; set; }
        
        public virtual ICollection<CartDetail> CartDetails { get; set; } = new HashSet<CartDetail>();
        
        public virtual OrderHeader OrderHeader { get; set; }
        [Required]
        public string UserId { get; set; }
        [ForeignKey("UserId")]
        public virtual ApplicationUser User { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}


