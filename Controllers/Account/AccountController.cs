using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Blink_API.Models;
using Blink_API.DTOs.IdentityDTOs.UserDTOs;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.Errors;
using Blink_API.Services.AuthServices;
using static System.Net.WebRequestMethods;

namespace Blink_API.Controllers.Account
{
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthServices _authServices;
        // add signIn manager for login : 
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailService _emailService;

        public AccountController(UserManager<ApplicationUser> userManager, IAuthServices authServices, SignInManager<ApplicationUser> signInManager, IEmailService emailService)
        {
            _userManager = userManager;
            _authServices = authServices;
            _signInManager = signInManager;
            _emailService = emailService;
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
        #region login
        [HttpPost("Login")] // api/account/Login
        public async Task<IActionResult> Login(LoginDto model)
        {
            if (ModelState.IsValid)
            {
                // find user :
                //var user = await _userManager.FindByEmailAsync(model.Email);
                //if (user == null)
                //{
                //    return Unauthorized(new ApiResponse(401, "Invalid email or password"));
                //}

                //var result = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

                //// check for login :
                //if (result.Succeeded)
                //{
                //    // valid user , generate el token :
                //    var token = await _authServices.CreateTokenAsync(user, _userManager);
                //    return Ok(new { Token = token });
                //}
                //else
                //{
                //    // invalid login :
                //    return Unauthorized(new ApiResponse(401, "Invalid login, Email or passward is not correct !"));
                //}


                var user = await _userManager.FindByNameAsync(model.Email);
                if (user == null)
                {
                    user= await _userManager.FindByEmailAsync(model.Email);
                }
                if ( user != null)
                {
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
            return BadRequest(new ApiResponse(400));
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
        [HttpPost("Forget passward")]
        public async Task<IActionResult> ForgotPassword([FromBody] ForgetPassward model)
        {
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                return BadRequest(new { message = "Email not found" });
            }
            var token = await _userManager.GeneratePasswordResetTokenAsync(user);

            // Create the reset password link :
            var resetLink = Url.Action("ResetPassword", "Account", new { token, email = user.Email }, Request.Scheme);

            // Send email
            await _emailService.SendEmailAsync(user.Email, "Reset Password",
                $"Click here to reset your password: <a href='https://localhost:7027/Reset-password?userId={user.Email}&token={token}'>Reset Password</a>");
            return Ok(new { message = "Password reset link has been sent to your email" });
        }
        #endregion

        #region Reset passward
        [HttpPost("Reset-password")]
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

        #endregion
    }
}
