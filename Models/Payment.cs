using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Payment
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PaymentId { get; set; }
        [Required]
        [StringLength(50)]
        public string Method { get; set; }
        [Required]
        [StringLength(50)]
        public string PaymentStatus { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentIntentId { get; set; }
        public virtual OrderHeader OrderHeader { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}


