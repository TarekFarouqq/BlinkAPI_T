using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class OrderHeader
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderHeaderId { get; set; }
        [Required]

        public DateTime OrderDate { get; set; }
        [Required]
        public decimal OrderSubtotal { get; set; }
        [Required]

        public decimal OrderTax { get; set; }
        [Required]

        public decimal OrderShippingCost { get; set; }
        [Required]

        public decimal OrderTotalAmount { get; set; }
        [Required]
        [StringLength(50)]
        public string OrderStatus { get; set; } 
        public virtual ICollection<OrderDetail> OrderDetails { get; set; } = new HashSet<OrderDetail>();
        public int PaymentId { get; set; }
        [ForeignKey("PaymentId")]

        public virtual Payment Payment { get; set; }
        // ضروري عشان نتبع الاوردر 
        public string? PaymentIntentId { get; set; }

        public int CartId { get; set; }

        [ForeignKey("CartId")]
        public virtual Cart Cart { get; set; }
        public bool IsDeleted { get; set; } = false;

    }
}


