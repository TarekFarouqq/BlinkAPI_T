using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class WishListDetail
    {
        public DateTime CreationDate { get; set; }
        public bool IsDeleted { get; set; } = false;

        public int WishListId { get; set; }
        [ForeignKey("WishListId")]
        public virtual WishList WishList { get; set; }
        public int ProductId { get; set; }

        [ForeignKey("ProductId")]
        public virtual Product Product { get; set; }


    }
}
