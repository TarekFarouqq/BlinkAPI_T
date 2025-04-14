using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Identity;

namespace Blink_API.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(15 ,MinimumLength =3)]
        public string FirstName { get; set; }
        [Required]
        [StringLength(15, MinimumLength = 3)]
        public string LastName { get; set; }
        public DateTime? LastModification { get; set; }
        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; set; }
        public DateTime CreatedIn { get; set; } = DateTime.UtcNow;
        public bool UserGranted { get; set; } // 
        public bool IsDeleted { get; set; } = false;
        public virtual WishList WishList { get; set; }
        public virtual Cart Cart { get; set; }
        public virtual ICollection<Review> Reviews { get; set; } = new HashSet<Review>();
        public virtual ICollection<Product> Products { get; set; } = new HashSet<Product>();
        public virtual ICollection<TransactionDetail> TransactionDetails { get; set; } = new HashSet<TransactionDetail>();
        public virtual ICollection<ReviewSuppliedProduct> ReviewsSuppliedProducts { get; set; }
    }
}
