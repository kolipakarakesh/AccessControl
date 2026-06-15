using AccessControl.Web.API.Helpers;
using AccessControl.Web.API.Models;
using AccessControl.Web.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace AccessControl.Web.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ProductController : ControllerBase
    {
        private readonly ILogger<ProductController> _logger;
        private readonly IProductService _productService;
        public ProductController(ILogger<ProductController> logger, IProductService productService)
        {
            _logger = logger;
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts()
        {
            try
            {
                var products = await _productService.GetProductsAsync();
                _logger.LogInformation("Retrieved {Count} products", products.Count);
                return Ok(products);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID.");
                }

                var product = await _productService.GetProductByIdAsync(id);

                _logger.LogInformation("Retrieved product with ID {ProductId}", id);

                if (product == null)
                {
                    return NotFound();
                }
                return Ok(product);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpPost]
        [AccessControlAutherise]
        public async Task<IActionResult> CreateProduct(Product product)
        {
            try
            {
                if (product == null)
                {
                    return BadRequest("Product object is null.");
                }
                var createdProduct = await _productService.CreateProductAsync(product);
                _logger.LogInformation("Created product with ID {ProductId}", createdProduct.ProductId);
                return Ok(createdProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
        [HttpPut]
        [Route("{id}")]
        [AccessControlAutherise]
        public async Task<IActionResult> UpdateProduct(int id, Product product)
        {
            try
            {
                if (id <= 0 || product == null)
                {
                    return BadRequest("Invalid product ID or product object is null.");
                }
                var updatedProduct = await _productService.UpdateProductAsync(id, product);
                if (updatedProduct == null)
                {
                    return NotFound();
                }
                _logger.LogInformation("Updated product with ID {ProductId}", id);
                return Ok(updatedProduct);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        [HttpDelete]
        [Route("{id}")]
        [AccessControlAutherise]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            try
            {
                if (id <= 0)
                {
                    return BadRequest("Invalid product ID.");
                }
                var deleted = await _productService.DeleteProductAsync(id);

                if (!deleted)
                {
                    return NotFound();
                }

                _logger.LogInformation("Deleted product with ID {ProductId}", id);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError($"Internal server error: {ex.Message}");
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
}
