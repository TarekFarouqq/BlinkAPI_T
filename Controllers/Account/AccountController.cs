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
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            var existUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null && existUserName !=null)
            {
                return BadRequest(new { 
                    StatusMessage ="failed",
                    message = "This email or userName are already exist.." 
                
                
                });
            }
            var user = new ApplicationUser()
            {
                FirstName = model.FName,
                LastName = model.LName,
                Email = model.Email,
                UserName = model.UserName,
                PhoneNumber = model.PhoneNumber,
                Address = model.Address,
                UserGranted = true,
               
                


            };


            var result = await _userManager.CreateAsync(user,model.Password);
            if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            await _userManager.UpdateAsync(user);
            return Ok(new UserDto()
            { 
                Message ="success",
                FullName = user.FirstName+" "+ user.LastName,
                UserName =user.UserName,
                Email = user.Email,
                Token = await  _authServices.CreateTokenAsync(user,_userManager),
                UserGranted = user.UserGranted ,
            
            
            
            });
        }

    }
}
