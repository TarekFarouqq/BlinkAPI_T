using Blink_API.Models;
using Microsoft.AspNetCore.Identity;

namespace Blink_API.Services.AuthServices
{
    public interface IAuthServices
    {
        Task<string> CreateTokenAsync(ApplicationUser user,UserManager<ApplicationUser> userManager);
    }
}
