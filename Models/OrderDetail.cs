using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class OrderDetail
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int OrderDetailId { get; set; }
        [Required]
        public int SellQuantity { get; set; }
        [Required]
        public decimal SellPrice { get; set; }
        [Required]
        public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        public virtual Product product { get; set; }
        [Required]

        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]

        public virtual OrderHeader OrderHeader { get; set; }
        public bool IsDeleted { get; set; } = false;


    }
}


