using Blink_API.DTOs.BranchDto;
using Blink_API.DTOs.InventoryDTOS;
using Blink_API.Errors;
using Blink_API.Services.BranchServices;
using Blink_API.Services.InventoryService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Inventory
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly InventoryService inventoryService;

        public InventoryController(InventoryService _inventoryService)
        {
            inventoryService = _inventoryService;
        }

        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var inventories = await inventoryService.GetAllInventories();
            return Ok(inventories);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var inventory = await inventoryService.GetInventoryById(id);
            if (inventory == null)
                return NotFound(new ApiResponse(404, "Inventory is Not Found"));
            return Ok(inventory);
        }

        [HttpPost("add")]
        public async Task<ActionResult> Add(AddInventoryDTO inventoryDTO)
        {
            var result = await inventoryService.AddInventory(inventoryDTO);
            return Ok(result);
        }


        [HttpPut("update/{id}")]
        public async Task<ActionResult> Update(int id, AddInventoryDTO inventoryDTO)
        {
            var result = await inventoryService.UpdateInventory(id, inventoryDTO);
            return Ok(result);
        }


        [HttpDelete("delete/{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var response = await inventoryService.DeleteInventory(id);
            if (response.StatusCode == 404)
            {
                return NotFound(new ApiResponse(404, "Inventory Not Found"));
            }
            return Ok(response);
        }

        [HttpGet("IsInventoryHasProducts/{inventoryId}")]
        public async Task<bool> IsInventoryHasProducts( int inventoryId)
        {
            var result = await inventoryService.IsInventoryHasProducts(inventoryId);
            return result;
        }
    }
}
