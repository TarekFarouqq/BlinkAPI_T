namespace Blink_API.DTOs.IdentityDTOs
{
    public class ResetPasswordDto
    {
        public string Token { get; set; } 
        public string Email { get; set; } 
        public string NewPassword { get; set; } 
    }
}
