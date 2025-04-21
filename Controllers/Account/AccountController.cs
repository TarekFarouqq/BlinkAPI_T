using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Blink_API.Models;
using Blink_API.DTOs.IdentityDTOs.UserDTOs;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.Errors;
using Blink_API.Services.AuthServices;
using static System.Net.WebRequestMethods;
using Microsoft.Extensions.Caching.Memory;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace Blink_API.Controllers.Account
{
    
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthServices _authServices;
        private readonly IMemoryCache _cache;
        // add signIn manager for login : 
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly EmailService _emailService;
        private AuthServiceUpdated authServiceUpdated;
        public AccountController(AuthServiceUpdated _authServiceUpdated,UserManager<ApplicationUser> userManager, IAuthServices authServices, SignInManager<ApplicationUser> signInManager, EmailService emailService, IMemoryCache cache)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
            _emailService = emailService;
            _cache = cache;
            authServiceUpdated = _authServiceUpdated;
        }

        #region Register 


        [HttpPost("register")] // api/account/register
        public async Task<ActionResult<UserDto>> Register(RegisterDto model)
        {
            var existingUser = await _userManager.FindByEmailAsync(model.Email);
            var existUserName = await _userManager.FindByNameAsync(model.UserName);
            if (existingUser != null && existUserName != null)
            {
                return BadRequest(new
                {
                    StatusMessage = "failed",
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





            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded is false) return BadRequest(new ApiResponse(400));
            await _userManager.UpdateAsync(user);
            return Ok(new UserDto()



            {
                Message = "success",
                FullName = user.FirstName + " " + user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Token = await _authServices.CreateTokenAsync(user, _userManager),
                UserGranted = user.UserGranted,




            });
        }



        #endregion

        #region login
        [HttpPost("Login")] // api/account/Login
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
 
                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    user= await _userManager.FindByEmailAsync(model.Email);
                }

              
                    var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);
                    if (result.Succeeded)
                    {
                        var token = await _authServices.CreateTokenAsync(user,_userManager);
                        return Ok(new {Token=token});
                    }
                    else
                    {
                        return Unauthorized(new ApiResponse(401, "Invalid Login, Login Details was Incorrect"));
                    }
                }
                else
                {
                    return NotFound("User Not Found");
                }

            }
        
        #endregion

        #region LogOut 
        [HttpPost("logout")] // api/account/logout
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();  

            return Ok(new { message = "Logged out successfully." });
        }
        #endregion

        #region ForgetPassward
        [HttpPost("forgetPassword")]
        public async Task<ActionResult<ApiResponse<object>>> ForgotPassword([FromBody] ForgetPassward model)
        {
            var result = await _emailService.SendResetCodeAsync(model.Email);
            if (!result)
                return BadRequest(new ApiResponse<object>(400, "Email not found"));

            return Ok(new ApiResponse<object>(200, "Code sent successfully"));
        }

        #endregion


        #region verify code
        [HttpPost("verifyCode")]
        public async Task<ActionResult<ApiResponse<object>>> VerifyCode([FromBody] VerifyCodeDto model)
        {
            var result = await _emailService.VerifyCodeAsync(model.Email, model.Code);
            if (!result)
                return BadRequest(new ApiResponse<object>(400, "Invalid or expired code"));

            return Ok(new ApiResponse<object>(200, "Code verified successfully", new { isValid = true }));
        }

        #endregion


        // after forget pass and enter el email , verify with code sent : 

        // set new password :
        #region set new password
        [HttpPost("setNewPassword")]
        public async Task<ActionResult<ApiResponse<object>>> SetNewPassword([FromBody] SetNewPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user == null)
                return BadRequest(new ApiResponse<object>(400, "User not found"));

            if (model.NewPassword != model.ConfirmPassword)
                return BadRequest(new ApiResponse<object>(400, "Passwords do not match"));

            var result = await _userManager.RemovePasswordAsync(user);
            if (!result.Succeeded)
                return BadRequest(new ApiResponse<object>(400, "Unable to remove old password"));

            var updateResult = await _userManager.AddPasswordAsync(user, model.NewPassword);
            if (!updateResult.Succeeded)
                return BadRequest(new ApiResponse<object>(400, "Unable to update password"));

            // Generate JWT token
            var token = await _authServices.CreateTokenAsync(user, _userManager); 

            return Ok(new ApiResponse<object>(200, "Password has been successfully updated", new { token }));
        }


        #endregion


        #region Admin Process
        [HttpPost("RegisterAdmin")]
        public async Task<ActionResult> RegisterAdmin(RegisterDto registerDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (registerDTO == null)
                return BadRequest();
            var result = await authServiceUpdated.RegisterAdmin(registerDTO);
            if (result == null || result == "")
                return BadRequest();
            if (result == "Username Is Registered Before")
                return BadRequest(result);
            if (result == "EmailAddress Is Registered Before")
                return BadRequest(result);
            return Ok(result);
        }
        #endregion
        #region Supplier Process
        [HttpPost("RegisterSupplier")]
        public async Task<ActionResult> RegisterSupplier(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (registerDto == null)
                return BadRequest();
            var result = await authServiceUpdated.RegisterSupplier(registerDto);
            if (result == null || result == "")
                return BadRequest();
            if (result == "Username Is Registered Before")
                return BadRequest(result);
            if (result == "EmailAddress Is Registered Before")
                return BadRequest(result);
            return Ok(result);
        }
        #endregion
        #region Client Process
        [HttpPost("RegisterClient")]
        public async Task<ActionResult> RegisterClient(RegisterDto registerDto)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (registerDto == null)
                return BadRequest();
            var result = await authServiceUpdated.RegisterClient(registerDto);
            if (result == null || result == "")
                return BadRequest();
            if (result == "Username Is Registered Before")
                return BadRequest(result);
            if (result == "EmailAddress Is Registered Before")
                return BadRequest(result);
            return Ok(result);
        }
        #endregion
        #region LoginProcess
        [HttpPost("LoginAccount")]
        public async Task<ActionResult> LoginAccount(LoginDto loginDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest();
            if (loginDTO == null)
                return BadRequest();
            var result = await authServiceUpdated.LoginAccount(loginDTO);
            if (result == null || result == "")
                return BadRequest();
            if (result == "Login details are incorrect")
                return BadRequest(result);
            return Ok(result);
        }
        #endregion
    }
}
