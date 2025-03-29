using Blink_API.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly ProductService productService;
        public ProductController(ProductService _productService  )
        {
            productService = _productService;
        }
        [HttpGet]
        public async Task<ActionResult> GetAllProducts()
        {
            var products = await productService.GetAllProducts();
            if (products == null)
                return NotFound();
            return Ok(products);
        }
        [HttpGet("GetById")]
        public async Task<ActionResult> GetProductById(int id)
        {
            var product = await productService.GetProductById(id);  
            if(product == null)
                return NotFound();
            return Ok(product);
        }
        
    }
}
