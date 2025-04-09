using Blink_API.Models;
using Blink_API.Services.Product;
using Microsoft.AspNetCore.Mvc;

namespace Blink_API.Controllers.Product
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
        public async Task<ActionResult> GetAll()
        {
            var products = await productService.GetAll();
            if (products == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var product in products)
            {
                product.ProductImages = product.ProductImages.Select(img => $"{baseUrl}{img.Replace("wwwroot/", "")}").ToList();
            }
            return Ok(products);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var product = await productService.GetById(id);  
            if(product == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            product.ProductImages = product.ProductImages.Select(img => $"{baseUrl}{img.Replace("wwwroot/", "")}").ToList();
            return Ok(product);
        }
        [HttpGet("GetByChildCategory/{id}")]
        public async Task<ActionResult> GetByChildCategory(int id)
        {
            var products = await productService.GetByChildCategoryId(id);
            if (products == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var product in products)
            {
                product.ProductImages = product.ProductImages.Select(img => $"{baseUrl}{img.Replace("wwwroot/", "")}").ToList();
            }
            return Ok(products);
        }
        [HttpGet("GetByParentCategory/{id}")]
        public async Task<ActionResult> GetByParentCategory(int id)
        {
            var products = await productService.GetByParentCategoryId(id);
            if (products == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var product in products)
            {
                product.ProductImages = product.ProductImages.Select(img => $"{baseUrl}{img.Replace("wwwroot/", "")}").ToList();
            }
            return Ok(products);
        }
    }
}
