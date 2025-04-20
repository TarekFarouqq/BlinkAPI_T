namespace Blink_API.Services.AuthServices
{
    public interface IEmailService 
    {
        Task SendEmailAsync(string email, string subject, string message);
        Task<bool> SendResetCodeAsync(string email);
        Task<bool> VerifyCodeAsync(string email, string code);
        Task<bool> SetNewPasswordAsync(string email, string newPassword);
    }
}
