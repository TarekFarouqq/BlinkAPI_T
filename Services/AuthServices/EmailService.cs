using System.Net.Mail;
using System.Net;

namespace Blink_API.Services.AuthServices
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var smtpClient = new SmtpClient(_configuration["EmailSettings:SmtpServer"])
            {
                Port = int.Parse(_configuration["EmailSettings:SmtpPort"]),
                UseDefaultCredentials = false,

                Credentials = new NetworkCredential(
                    _configuration["EmailSettings:SenderEmail"],
                    _configuration["EmailSettings:SenderPassword"]),
                EnableSsl = true,
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderName"]),
                Subject = subject,
                Body = message,
                IsBodyHtml = true,
            };
            mailMessage.To.Add(email);

            await smtpClient.SendMailAsync(mailMessage);
        }
    
}
}
