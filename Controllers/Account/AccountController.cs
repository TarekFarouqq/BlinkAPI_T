using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Blink_API.Models;
using Blink_API.DTOs.IdentityDTOs.UserDTOs;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.Errors;
using Blink_API.Services.AuthServices;
namespace Blink_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthServices _authServices;

        public AccountController(UserManager<ApplicationUser> userManager,IAuthServices authServices)
        {
            _userManager = userManager;
            _authServices = authServices;
        }


        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var user = new ApplicationUser()
            {
                FirstName = model.FName,
                LastName = model.LName,
                Email = model.Email,
                UserName = model.Email.Split("@")[0],
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,


            };

            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            return Ok(new UserDto()
            { 
                FullName = user.FirstName+user.LastName,
                Email = user.Email,
                Token = await  _authServices.CreateTokenAsync(user,_userManager)
            
            
            
            });
        }

    }
}
