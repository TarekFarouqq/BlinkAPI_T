using System.ComponentModel.DataAnnotations;

namespace Blink_API.DTOs.IdentityDTOs
{
    public class RegisterDto
    {
        [Required]
        public string FName { get; set; }
        [Required]
        public string LName { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string PhoneNumber { get; set; }
        [Required]
        [RegularExpression("^(?=.*\\d)(?=.*[a-z])(?=.*[A-Z])(?=.*[a-zA-Z]).{8,}$"
            , ErrorMessage = "Password must be at least 8 characters long and include at least one uppercase letter," +" one digit, and one special character (@$!%*?&")]
        public string Password { get; set; }

        [Required]
        [StringLength(200, MinimumLength = 5)]
        public string Address { get; set; }


    }
}
