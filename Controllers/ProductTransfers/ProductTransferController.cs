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
            productTransferService= _productTransferService;
        }

    }
}
