using Blink_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.BiDataDtos
{
    public class Discount_DimensionDto
    {
        public int DiscountId { get; set; }
       
        public int ProductId { get; set; }
        public int DiscountPercentage { get; set; }
        public decimal DiscountAmount { get; set; }
        public DateTime DiscountFromDate { get; set; }
   
        public DateTime DiscountEndDate { get; set; }

    }
}
