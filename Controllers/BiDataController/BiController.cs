using Blink_API.Services.BiDataService;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.BiDataController
{
    [Route("api/[controller]")]
    [ApiController]
    public class BiController : ControllerBase
    {
        public readonly BiStockService _biStockService;

        public BiController(BiStockService biStockService)
        {
            _biStockService = biStockService;
        }

        // Stock_Fact
        [HttpGet]
        [Route("GetAllStockFacts")]
        public  async Task<ActionResult> GetAllStockFacts()
        {
            var stockFacts = await _biStockService.GetAllStockFacts();
            return Ok(stockFacts);
        }
        // review_diminsion
        [HttpGet]
        [Route("GetAllReviewDimensions")]
        public async Task<ActionResult> GetAllReviewDimensions()
        {
            var reviewDimensions = await _biStockService.GetAllReviewDimensions();
            return Ok(reviewDimensions);
        }

        // payment_diminsion
        [HttpGet]
        [Route("GetAllPayments")]
        public async Task<ActionResult> GetAllPayments()
        {
            var payments = await _biStockService.GetAllPayments();
            return Ok(payments);
        }

        // user_roles_diminsion
        [HttpGet]
        [Route("GetAllUserRoles")]
        public async Task<ActionResult> GetAllUserRoles()
        {
            var userRoles = await _biStockService.GetAllUserRoles();
            return Ok(userRoles);
        }

        // all roles :
        [HttpGet]
        [Route("GetAllRoles")]
        public async Task<ActionResult> GetAllRoles()
        {
            var roles = await _biStockService.GetAllRoles();
            return Ok(roles);
        }

        // all users :
        [HttpGet]
        [Route("GetAllUsers")]
        public async Task<ActionResult> GetAllUsers()
        {
            var users = await _biStockService.GetAllUsers();
            return Ok(users);
        }

        // all product discont :
        [HttpGet]
        [Route("GetAllProductDiscounts")]
        public async Task<ActionResult> GetAllProductDiscounts()
        {
            var productDiscounts = await _biStockService.GetAllProductDiscounts();
            return Ok(productDiscounts);
        }

        // get all inventory transaction :
        [HttpGet]
        [Route("GetAllInventoryTransaction")]
        public async Task<ActionResult> GetAllInventoryTransaction()
        {
            var inventoryTransactions = await _biStockService.GetAllInventoryTransactions();
            return Ok(inventoryTransactions);
        }

        // get all carts :
        [HttpGet]
        [Route("GetAllCarts")]
        public async Task<ActionResult> GetAllCarts()
        {
            var carts = await _biStockService.GetAllCarts();
            return Ok(carts);
        }

        // get all order details :
        [HttpGet]
        [Route("GetAllOrderDetails")]
        public async Task<ActionResult> GetAllOrderDetails()
        {
            var orderDetails = await _biStockService.GetAllOrderFacts();
            return Ok(orderDetails);
        }
        // get all discountes:
        [HttpGet]
        [Route("GetAllDiscounts")]
        public async Task<ActionResult> GetAllDiscounts()
        {
            var discounts = await _biStockService.GetAllDiscounts();
            return Ok(discounts);
        }

        // get all branch inventory :
        [HttpGet]
        [Route("GetAllBranchInventory")]
        public async Task<ActionResult> GetAllBranchInventory()
        {
            var branchInventory = await _biStockService.GetAllInventoryBranches();
            return Ok(branchInventory);
        }
        // get all inventory transactionfact  :
        [HttpGet]
        [Route("GetAllInventoryTransactionFact")]
        public async Task<ActionResult> GetAllInventoryTransactionFact()
        {
            var inventoryTransactionFact = await _biStockService.GetAllInventoryTransactionFacts();
            return Ok(inventoryTransactionFact);
        }

        // get all product diminsion :
        [HttpGet]
        [Route("GetAllProducts")]
        public async Task<ActionResult> GetAllProducts()
        {
            var productDiminsion = await _biStockService.GetAllProductDimensions();
            return Ok(productDiminsion);
        }
    }
}
