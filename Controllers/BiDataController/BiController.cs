using Blink_API.DTOs.BiDataDtos;
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
        [Route("GetAllStockFactsInChunks")]
        public async IAsyncEnumerable<List<stock_factDto>> GetAllStockFactsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllStockFactsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllStockFacts")]
        //public  async Task<ActionResult> GetAllStockFacts()
        //{
        //    var stockFacts = await _biStockService.GetAllStockFacts();
        //    return Ok(stockFacts);
        //}

        #endregion
        // review_diminsion
        [HttpGet]
        [Route("GetAllReviewDimensionsInChunks")]
        public async IAsyncEnumerable<List<Review_DimensionDto>> GetAllReviewDimensionsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllReviewDimensionsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllReviewDimensions")]
        //public async Task<ActionResult> GetAllReviewDimensions()
        //{
        //    var reviewDimensions = await _biStockService.GetAllReviewDimensions();
        //    return Ok(reviewDimensions);
        //}
        #endregion
        // payment_diminsion
        //[HttpGet]
        //[Route("GetAllPaymentsInChunks")]
        //public async IAsyncEnumerable<List<Payment_DimensionDto>> GetAllPaymentsInChunks()
        //{
        //    await foreach (var chunk in _biStockService.GetAllPaymentsInChunks())
        //    {
        //        yield return chunk;
        //    }
        //}

        #region old
        [HttpGet]
        [Route("GetAllPayments")]
        public async Task<ActionResult> GetAllPayments()
        {
            var payments = await _biStockService.GetAllPayments();
            return Ok(payments);
        }
        #endregion
        // user_roles_diminsion
        [HttpGet]
        [Route("GetAllUserRolesInChunks")]
        public async IAsyncEnumerable<List<UserRoles_DimensionDto>> GetAllUserRolesInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllUserRolesInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllUserRoles")]
        //public async Task<ActionResult> GetAllUserRoles()
        //{
        //    var userRoles = await _biStockService.GetAllUserRoles();
        //    return Ok(userRoles);
        //}
        #endregion
        // all roles :

        [HttpGet]
        [Route("GetAllRolesInChunks")]
        public async IAsyncEnumerable<List<Role_DiminsionDto>> GetAllRolesInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllRolesInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllRoles")]
        //public async Task<ActionResult> GetAllRoles()
        //{
        //    var roles = await _biStockService.GetAllRoles();
        //    return Ok(roles);
        //}
        #endregion
        // all users :
        //[HttpGet]
        //[Route("GetAllUsers")]
        //public async Task<ActionResult> GetAllUsers()
        //{
        //    var users = await _biStockService.GetAllUsers();
        //    return Ok(users);
        //}
        //  get all users in chunks :
        [HttpGet]
        [Route("GetAllUsersInChunks")]
        public async IAsyncEnumerable<List<User_DimensionDto>> GetAllUsersInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllUsersInChunks())
            {
                yield return chunk;
            }
        }


        /// /////////////////////////////////////////////////


        // all product discont :
        [HttpGet]
        [Route("GetAllProductDiscountsInChunks")]
        public async IAsyncEnumerable<List<Product_DiscountDto>> GetAllProductDiscountsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllProductDiscountsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllProductDiscounts")]
        //public async Task<ActionResult> GetAllProductDiscounts()
        //{
        //    var productDiscounts = await _biStockService.GetAllProductDiscounts();
        //    return Ok(productDiscounts);
        //}
        #endregion

        // get all inventory transaction :
        [HttpGet]
        [Route("GetAllInventoryTransactionInChunks")]
        public async IAsyncEnumerable<List<Inventory_Transaction_Dto>> GetAllInventoryTransactionInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllInventoryTransactionsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllInventoryTransaction")]
        //public async Task<ActionResult> GetAllInventoryTransaction()
        //{
        //    var inventoryTransactions = await _biStockService.GetAllInventoryTransactions();
        //    return Ok(inventoryTransactions);
        //}
        #endregion
        // get all carts :
        [HttpGet]
        [Route("GetAllCartsInChunks")]
        public async IAsyncEnumerable<List<cart_DiminsionDto>> GetAllCartsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllCartsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllCarts")]
        //public async Task<ActionResult> GetAllCarts()
        //{
        //    var carts = await _biStockService.GetAllCarts();
        //    return Ok(carts);
        //}
        #endregion
        // get all order details :
        [HttpGet]
        [Route("GetAllOrderDetailsInChunks")]
        public async IAsyncEnumerable<List<order_FactDto>> GetAllOrderFactsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllOrderFactsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllOrderDetails")]
        //public async Task<ActionResult> GetAllOrderDetails()
        //{
        //    var orderDetails = await _biStockService.GetAllOrderFacts();
        //    return Ok(orderDetails);
        //}
        #endregion
        // get all discountes:

        [HttpGet]
        [Route("GetAllDiscountsInChunks")]
        public async IAsyncEnumerable<List<Discount_DimensionDto>> GetAllDiscountsInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllDiscountsInChunks())
            {
                yield return chunk;
            }
        }


        #region old 
        //[HttpGet]
        //[Route("GetAllDiscounts")]
        //public async Task<ActionResult> GetAllDiscounts()
        //{
        //    var discounts = await _biStockService.GetAllDiscounts();
        //    return Ok(discounts);
        //}
        #endregion
        // get all branch inventory :
        [HttpGet]
        [Route("GetAllBranchInventoryInChunks")]
        public async IAsyncEnumerable<List<Branch_inventoryDto>> GetAllBranchInventoryInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllInventoryBranchesInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllBranchInventory")]
        //public async Task<ActionResult> GetAllBranchInventory()
        //{
        //    var branchInventory = await _biStockService.GetAllInventoryBranches();
        //    return Ok(branchInventory);
        //}
        #endregion
        // get all inventory transactionfact  :
        [HttpGet]
        [Route("GetAllInventoryTransactionFactInChunks")]
        public async IAsyncEnumerable<List<InventoryTransaction_FactDto>> GetAllInventoryTransactionFactInChunks()
        {
            await foreach (var chunk in _biStockService.GetAllInventoryTransactionFactsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllInventoryTransactionFact")]
        //public async Task<ActionResult> GetAllInventoryTransactionFact()
        //{
        //    var inventoryTransactionFact = await _biStockService.GetAllInventoryTransactionFacts();
        //    return Ok(inventoryTransactionFact);
        //}
        #endregion

        // get all product diminsion :
        [HttpGet]
        [Route("GetAllProducts")]
        public async IAsyncEnumerable<List<Product_DiminsionDto>> GetAllProducts()
        {
            await foreach (var chunk in _biStockService.GetAllProductDimensionsInChunks())
            {
                yield return chunk;
            }
        }

        #region old
        //[HttpGet]
        //[Route("GetAllProducts")]
        //public async Task<ActionResult> GetAllProducts()
        //{
        //    var productDiminsion = await _biStockService.GetAllProductDimensions();
        //    return Ok(productDiminsion);
        //}
        #endregion
    }
}
