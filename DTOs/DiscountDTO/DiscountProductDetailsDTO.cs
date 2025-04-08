using Blink_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.DiscountDTO
{
    public class DiscountProductDetailsDTO
    {
        public int DiscountId { get; set; }
        public int ProductId { get; set; }
        public decimal DiscountAmount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
