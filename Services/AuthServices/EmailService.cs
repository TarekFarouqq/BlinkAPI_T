using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Identity;
using Blink_API.Models;
using Microsoft.Extensions.Caching.Memory;

namespace Blink_API.Services.AuthServices
{
    public class EmailService:IEmailService
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMemoryCache _cache;
        public EmailService(IConfiguration configuration, UserManager<ApplicationUser> userManager, IMemoryCache cache)
        {
            _configuration = configuration;
            _userManager = userManager;
            _cache = cache;
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


        public async Task<bool> SendResetCodeAsync(string email)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var code = new Random().Next(100000, 999999).ToString();
            _cache.Set(email, code, TimeSpan.FromMinutes(5));

            await SendEmailAsync(email, "Password Reset Code Dont't Share it with any one", $"Your reset code is: {code}" );
            return true;
        }
        public Task<bool> VerifyCodeAsync(string email, string code)
        {
            var cachedCode = _cache.Get<string>(email);
            if (cachedCode == null || cachedCode != code)
                return Task.FromResult(false);

            _cache.Set(email + "_verified", true, TimeSpan.FromMinutes(10));
            return Task.FromResult(true);
        }

        public async Task<bool> SetNewPasswordAsync(string email, string newPassword)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null) return false;

            var removeResult = await _userManager.RemovePasswordAsync(user);
            if (!removeResult.Succeeded) return false;

            var addResult = await _userManager.AddPasswordAsync(user, newPassword);
            return addResult.Succeeded;
        }


    }
}
