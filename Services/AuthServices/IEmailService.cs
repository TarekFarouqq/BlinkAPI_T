namespace Blink_API.Services.AuthServices
{
    public interface IEmailService 
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
