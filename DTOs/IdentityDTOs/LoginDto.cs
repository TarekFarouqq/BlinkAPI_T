using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.IdentityDTOs
{
    public class LoginDto
    {
        [Required]
        public string Email { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"
            , ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter," + " one digit, and one special character (@$!%*?&")]
        public string Password { get; set; }
    }
}
