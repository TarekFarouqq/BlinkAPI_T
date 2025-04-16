using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Blink_API.Models
{
    public class Advertisment
    {
        [Key,DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int AdvertismentId { get; set; }
        [Required]
        public string ImagePath { get; set; }
        [Required]
        public int Position { get; set; }
        public bool isDeleted { get; set; } = false;
    }
}
