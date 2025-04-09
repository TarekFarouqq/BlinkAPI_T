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
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, IAuthServices authServices, SignInManager<ApplicationUser> signInManager, IEmailService emailService, IMemoryCache cache)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
            _emailService = emailService;
            _cache = cache;
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
        [HttpPost("forgetPassward")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPassward model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null || user.Email == null)
            {
                return BadRequest(new { message = "Email not found" });
            }
            //var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            //// Create the reset password link :
            //var resetLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token }, Request.Scheme);

            //// Send email
            //await _emailService.SendEmailAsync(user.Email, "Reset Password",
            //    $"Click here to reset your password: <a href='https://localhost:7027/Reset-password?userId={user.Email}&token={token}'>Reset Password</a>");
            //return Ok(new { message = "Password reset link has been sent to your email" });
            //---------------------------------
            // Create the reset password code of 6 nums : 
            var verificationCode = new Random().Next(100000, 999999).ToString();


            _cache.Set(model.Email, verificationCode, TimeSpan.FromMinutes(5));

            // Send email
            await _emailService.SendEmailAsync(user.Email, "Password Reset Code",
                $"Your reset code is: {verificationCode}");

            return Ok(new { message = "Password reset code has been sent to your email" });
        }
        #endregion

        #region Reset passward
        /*
        [HttpPost("Resetpassword")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDto model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Invalid email, please try agin !" });
            }

            var result = await _userManager.ResetPasswordAsync(user, model.Token, model.NewPassword);
            if (result.Succeeded)
            {
                return Ok(new { message = "Password has been reset successfully" });
            }

            return BadRequest(new { message = "Enable to reset password" });
        }
        */

        #endregion

        // after forget pass and enter el email , verify with code sent : 
        #region verify code
        [HttpPost("verifyCode")]
        public async Task<IActionResult> VerifyCode([FromBody] VerifyCodeDto model)
        {
            if (!_cache.TryGetValue(model.Email, out string savedCode))
            {
                return BadRequest(new { message = "Code expired or not found", isValid = false });
            }

            if (model.code == savedCode)
            {
                return Ok(new { message = "Valid code", isValid = true });
            }
            else
            {
                return BadRequest(new { message = "Invalid code", isValid = false });
            }
        }
        #endregion

        // set new password :
        #region set new password

        [HttpPost("setNewPassword")]
        public async Task<IActionResult> SetNewPassword([FromBody] SetNewPasswordDto model)
        {

            var user = await _userManager.GetUserAsync(User);   

            if (user == null)
            {
                return BadRequest(new { message = "User not found" });
            }

            // check matching passwords :
            if (model.NewPassword != model.ConfirmPassword)
            {
                return BadRequest(new { message = "Passwords do not match" });
            }

      // remove old pass and update it then:
            var result = await _userManager.RemovePasswordAsync(user);  
            if (result.Succeeded)
            {
                var updateResult = await _userManager.AddPasswordAsync(user, model.NewPassword);  
                if (updateResult.Succeeded)
                {
                    return Ok(new { message = "Password has been successfully updated" });
                }
                else
                {
                    return BadRequest(new { message = "Unable to update password" });
                }
            }
            else
            {
                return BadRequest(new { message = "Unable to remove old password" });
            }
        }
        #endregion
    }
}
