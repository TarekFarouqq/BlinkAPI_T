using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Branch
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BranchId { get; set; }
        [Required]
        [StringLength(50)]
        public string BranchName { get; set; }
        [Required]
        [StringLength(200)]
        public string BranchAddress { get; set; }
        [Required]
        [Phone]
        public string Phone { get; set; }
        public bool IsDeleted { get; set; } = false;
        public virtual ICollection<Inventory> Inventories { get; set; } = new HashSet<Inventory>();

    }
}


