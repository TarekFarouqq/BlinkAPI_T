using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.IdentityDTOs
{
    public class ResetPasswordDto
    {
        [Required]
        public string Token { get; set; }
        [Required]
        [EmailAddress(ErrorMessage = "Invalid Email")]
        public string Email { get; set; }

        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[\\W_]).{8,}$", ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter, one digit, and one special character (@$!%*?&)")]

        public string NewPassword { get; set; } 
    }
}
