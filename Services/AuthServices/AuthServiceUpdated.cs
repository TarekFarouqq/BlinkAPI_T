using System.IdentityModel.Tokens.Jwt;
using System.Security.AccessControl;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Blink_API.DTOs.IdentityDTOs;
using Blink_API.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Blink_API.Services.AuthServices
{
    public class AuthServiceUpdated
    {
        private UserManager<ApplicationUser> userManager;
        private RoleManager<IdentityRole> roleManager;
        private SignInManager<ApplicationUser> signInManager;
        private IMapper mapper;
        private readonly IConfiguration _configuration;
        public AuthServiceUpdated(
            UserManager<ApplicationUser> _userManager,
            RoleManager<IdentityRole> _roleManager,
            SignInManager<ApplicationUser> _signInManager,
            IMapper _mapper,
            IConfiguration _configuration)
        {
            userManager = _userManager;
            roleManager = _roleManager;
            signInManager = _signInManager;
            mapper = _mapper;
            this._configuration = _configuration;
        }
        private async Task<string> CreateTokenAsync(ApplicationUser admin)
        {
            if (admin != null)
            {
                var authClaims = new List<Claim>()
                {
                new Claim(ClaimTypes.Name,admin.UserName),
                new Claim(ClaimTypes.Email,admin.Email),
                new Claim(ClaimTypes.NameIdentifier,admin.Id),
                };
                var userRoles = await userManager.GetRolesAsync(admin);
                foreach (var role in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, role));
                }
                var authKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:SecretKey"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(double.Parse(_configuration["JWT:DurationInDays"])),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authKey, SecurityAlgorithms.HmacSha256Signature)
                    );
                return new JwtSecurityTokenHandler().WriteToken(token);
            }
            return null;
        }
        private async Task<bool> CheckRule(string ruleName)
        {
            var roleExist = await roleManager.RoleExistsAsync(ruleName);
            if (!roleExist)
            {
                var roleResult = await roleManager.CreateAsync(new IdentityRole(ruleName));
                if (!roleResult.Succeeded)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<object> RegisterAdmin(RegisterDto registerAdmin)
        {
            var existingUsername = await userManager.FindByNameAsync(registerAdmin.UserName);
            if (existingUsername != null)
                return "Username Is Registered Before";
            var existingEmail = await userManager.FindByEmailAsync(registerAdmin.Email);
            if (existingEmail != null)
                return "EmailAddress Is Registered Before";
            if (await CheckRule("Admin"))
            {
                var mappedAdmin = mapper.Map<ApplicationUser>(registerAdmin);
                var admin = await userManager.CreateAsync(mappedAdmin, registerAdmin.Password);
                if (admin.Succeeded)
                {
                    await userManager.AddToRoleAsync(mappedAdmin, "Admin");
                    var token = await CreateTokenAsync(mappedAdmin);
                    return new
                    {
                        token = token,
                        username=mappedAdmin.UserName,
                        email=mappedAdmin.Email
                    };
                }
            }
            return "";
        }
        public async Task<object> RegisterSupplier(RegisterDto registerSupplier)
        {
            var existingUsername = await userManager.FindByNameAsync(registerSupplier.UserName);
            if (existingUsername != null)
                return "Username Is Registered Before";
            var existingEmail = await userManager.FindByEmailAsync(registerSupplier.Email);
            if (existingEmail != null)
                return "EmailAddress Is Registered Before";
            if (existingUsername != null && existingEmail != null)
                return "";
            if (await CheckRule("Supplier"))
            {
                var mappedSupplier = mapper.Map<ApplicationUser>(registerSupplier);
                var supplier = await userManager.CreateAsync(mappedSupplier, registerSupplier.Password);
                if(supplier.Succeeded)
                {
                    await userManager.AddToRoleAsync(mappedSupplier, "Supplier");
                    var token = await CreateTokenAsync(mappedSupplier);
                    return new
                    {
                        token = token,
                        username=mappedSupplier.UserName,
                        email=mappedSupplier.Email
                    };
                }
            }
            return "";
        }
        public async Task<object> RegisterClient(RegisterDto registerClient)
        {
            var existingUsername = await userManager.FindByNameAsync(registerClient.UserName);
            if (existingUsername != null)
                return "Username Is Registered Before";
            var existingEmail = await userManager.FindByEmailAsync(registerClient.Email);
            if (existingEmail != null)
                return "EmailAddress Is Registered Before";
            if (existingUsername != null && existingEmail != null)
                return "";
            if (await CheckRule("Client"))
            {
                var mappedSupplier = mapper.Map<ApplicationUser>(registerClient);
                var supplier = await userManager.CreateAsync(mappedSupplier, registerClient.Password);
                if (supplier.Succeeded)
                {
                    await userManager.AddToRoleAsync(mappedSupplier, "Client");
                    var token = await CreateTokenAsync(mappedSupplier);
                    return new
                    {
                        token = token,
                        username = mappedSupplier.UserName,
                        email = mappedSupplier.Email
                    };
                }
            }
            return "";
        }
        public async Task<object> LoginAccount(LoginDto loginAccount)
        {
            var user = await userManager.FindByEmailAsync(loginAccount.Email)
                       ?? await userManager.FindByNameAsync(loginAccount.Email);
            if (user == null)
                return "Login details are incorrect";
            var signInResult = await signInManager.PasswordSignInAsync(user.UserName, loginAccount.Password, false, false);
            if (!signInResult.Succeeded)
                return "Login details are incorrect";
            var token = await CreateTokenAsync(user);
            return new
            {
                token = token,
                username=user.UserName,
                email=user.Email
            };
        }
    }
}
