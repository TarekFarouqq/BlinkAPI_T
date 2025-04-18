using Blink_API.DTOs.TransferProductsDTOs;
using Blink_API.Services.ProductServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.ProductTransfers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductTransferController : ControllerBase
    {
        private readonly ProductTransferService productTransferService;
        public ProductTransferController(ProductTransferService _productTransferService)
        {
            productTransferService = _productTransferService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllTransactionHeader()
        {
            var result = await productTransferService.GetAllTransactionHeader();
            if (result == null)
                return NotFound(new { Message = "Not Transactions Found" });
            return Ok(result);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetTransactionHeaderById(int id)
        {
            var result = await productTransferService.GetTransactionHeaderById(id);
            if (result == null)
                return NotFound(new { Message = "Not Transaction Found" });
            return Ok(result);
        }
        [HttpPost("AddProductToInventory")]
        public async Task<ActionResult> AddInputInventory(InsertInputTransferProductDTO insertInputTransferProductDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model State is Invalid" });
            if (insertInputTransferProductDTO == null)
                return BadRequest(new { Message = "Model is null" });
            await productTransferService.AddInputInventory(insertInputTransferProductDTO);
            return Ok(new { Message = "Model Inserted Successfull" });
        }
        [HttpPost("CreateTransaction")]
        public async Task<ActionResult> CreateTransaction(InsertTransactionHistoryDTO insertTransactionHistoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model isnot Valid" });
            if (insertTransactionHistoryDTO == null)
                return BadRequest(new { Message = "Model is empty" });
            await productTransferService.CreateTransaction(insertTransactionHistoryDTO);
            return Ok(new { Message = "Transaction Created successfull" });
        }
        [HttpPut("UpdateTransaction/{id}")]
        public async Task<ActionResult> UpdateTransaction(int id, InsertTransactionHistoryDTO insertTransactionHistoryDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(new { Message = "Model isnot Valid" });
            if (insertTransactionHistoryDTO == null)
                return BadRequest(new { Message = "Model is empty" });
            await productTransferService.UpdateTransaction(id,insertTransactionHistoryDTO);
            return Ok(new { Message = "Transaction is Updated Successfull" });
        }
    }
}
