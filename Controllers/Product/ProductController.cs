using System.Reflection.Metadata.Ecma335;
using Blink_API.DTOs.ProductDTOs;
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
        [HttpGet("GetPagesCount/{pgSize}")]
        public async Task<ActionResult> GetPagesCount(int pgSize)
        {
            var count = await productService.GetPagesCount(pgSize);
            if (count == 0)
                return NotFound();
            return Ok(count);
        }
        [HttpGet("GetAllWithPaging/{pgNumber}/{pgSize}")]
        public async Task<ActionResult> GetAllPagginated(int pgNumber,int pgSize)
        {
            var products = await productService.GetAllPagginated(pgNumber,pgSize);
            if (products == null)
                return NotFound();
            string baseUrl = $"{Request.Scheme}://{Request.Host}/";
            foreach (var product in products)
            {
                product.ProductImages = product.ProductImages.Select(img => $"{baseUrl}{img.Replace("wwwroot/", "")}").ToList();
            }
            return Ok(products);
        }
        [HttpGet("GetFilteredProducts/{filter}/{pgNumber}/{pgSize}")]
        public async Task<ActionResult> GetFilteredProducts(string filter, int pgNumber, int pgSize)
        {
            var products = await productService.GetFilteredProducts(filter, pgNumber, pgSize);
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
        //[HttpPost]
        //public async Task<ActionResult> Add(InsertProductDTO productDTO)
        //{
        //    if(!ModelState.IsValid)
        //        return BadRequest(ModelState);
        //    if (productDTO == null)
        //        return BadRequest();
        //    var result = await productService.Add(productDTO);
        //    if (result == 0)
        //        return BadRequest();
        //    return Ok(result);
        //}
        [HttpPost]
        public async Task<ActionResult> Add(InsertProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (productDTO == null)
                return BadRequest();
            var result = await productService.Add(productDTO);
            if (result == 0)
                return BadRequest();
            int prdId = result;
            await productService.AddProductImage(prdId, productDTO.ProductImages);
            return Ok(result);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, InsertProductDTO productDTO)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            if (productDTO == null)
                return BadRequest();
            await productService.Update(id, productDTO);
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            if (id <= 0)
                return BadRequest();
            await productService.Delete(id);
            return Ok();
        }
    }
}
