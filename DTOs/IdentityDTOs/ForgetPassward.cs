using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.IdentityDTOs
{
    public class ForgetPassward
    {
        [Required(ErrorMessage = "Email Is Requried !")]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }
    }
}
