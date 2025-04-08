using Blink_API.DTOs.BranchDto;
using Blink_API.Errors;
using Blink_API.Models;
using Blink_API.Services.BranchServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.BranchController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly BranchServices _branchService;

        public BranchController(BranchServices branchService)
        {
            _branchService = branchService;
        }

        // GET: api/Branch
        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var branches = await _branchService.GetAllBranches();
            return Ok(branches);
        }

        // GET: api/Branch/5
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var branch = await _branchService.GetBranchById(id);
            if (branch == null )
                return NotFound(new ApiResponse(404,"Branch is Not Found"));
            return Ok(branch);
        }
        // POST: api/Branch/add
        [HttpPost("add")]
        public async Task<ActionResult> Add(AddBranchDTO branchdto)
        {
            var result = await _branchService.AddBranch(branchdto);
            return Ok(result);
        }
        // PUT: api/Branch/update/5
        [HttpPut("update/{id}")]
        public async Task<ActionResult> Update(int id, AddBranchDTO branchdto)
        {
            var result = await _branchService.UpdateBranch(id, branchdto);
            return Ok(result);
        }

        // DELETE: api/Branch/delete/5
        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await _branchService.DeleteBranch(id);
            if (response.StatusCode == 404)
            {
                return NotFound(new ApiResponse(404,"Branch Not Found"));
            }
            return Ok(response);
        }

    }
}
