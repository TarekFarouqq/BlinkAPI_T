using Blink_API.DTOs.UsersDtos;
using Blink_API.Errors;
using Blink_API.Services.UserService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Users
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly UserService userService;
        public UsersController(UserService _userService)
        {
            userService = _userService;
        }

        // get all users:
        [HttpGet("GetAll")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await userService.GetAllUsers();
            if (users == null)
                return NotFound();
            return Ok(users);
        }

        // get user by id:
        [HttpGet("GetById/{id}")]
        public async Task<ActionResult> GetUserById(string id)
        {
            var user = await userService.GetUserById(id);
            if (user == null)
                return NotFound(new ApiResponse(404, "User not found"));
            return Ok(user);
        }

        // get user by name:
        [HttpGet("GetByUserName/{name}")]
        public async Task<ActionResult> GetUserByName(string name)
        {
            var users = await userService.GetUserByName(name);
            if (users == null || !users.Any())
                return NotFound(new ApiResponse(404, " No users found with this name"));
            return Ok(users);
        }
        // insert user:
        [HttpPost("Insert")]
        public async Task<ActionResult> InsertUser(AddUserDto user)
        {
            
            var result = await userService.InsertUser(user);
            
            return Ok(result);
        }

        // update user:
        [HttpPut("Update/{id}")]
        public async Task<ActionResult> UpdateUser(string id, AddUserDto user)
        {
            var result = await userService.UpdateUser(id, user);
             
            return Ok(result);
        }

        // delete user:
        [HttpDelete("Delete/{id}")]
        public async Task<ActionResult> DeleteUser(string id)
        {
            var result = await userService.SoftDeleteUser(id);
             
            return Ok(result);
        }

        [HttpGet("GetAllPaginated")]
        public async Task<ActionResult> GetAllUsersPaginated(int pageNumber = 1, int pageSize = 10)
        {
            var users = await userService.GetAllUsersPaginated(pageNumber, pageSize);
            if (users == null || !users.Any())
                return NotFound(new ApiResponse(404, "No users found."));
            return Ok(users);
        }

        [HttpGet("GetPagesCount")]
        public async Task<ActionResult> GetPagesCount(int pageSize = 10)
        {
            var count = await userService.GetPagesCount(pageSize);
            return Ok(count);
        }

    }
}
