using Blink_API.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.DiscountDTO
{
    public class DiscountDetailsDTO
    {
        public int DiscountId { get; set; }
        public int DiscountPercentage { get; set; }
        public DateTime DiscountFromDate { get; set; }
        public DateTime DiscountEndDate { get; set; }
        public bool IsDeleted { get; set; } = false;
        public ICollection<DiscountProductDetailsDTO> DiscountProducts { get; set; }=new List<DiscountProductDetailsDTO>();
    }
}
