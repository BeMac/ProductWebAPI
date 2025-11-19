using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductWebApi.Models;
using ProductWebApi.Repositories;
using ProductWebApi.Services;

namespace ProductWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductService _productService;

        public ProductsController(IProductRepository productProductRepository, IProductService productService)
        {
            _productRepository = productProductRepository;
            _productService = productService;
        }

        
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return Ok(await _productRepository.GetAllAsync());
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _productRepository.GetProductAsync(id);

            if (product == null)
            {
                return NotFound();
            }

            return product;
        }
                
        [HttpPut("{id}")]
        public async Task<IActionResult> PutProduct(int id, [FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
            var product = new Product
            {
                Id = id,
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                IsActive = request.IsActive                
            };

            var updatedProduct = await _productRepository.UpdateAsync(id, product);

            if (updatedProduct == null)
            {
                return NotFound();
            }

            return NoContent();
            
        }
        
        [HttpPost]
        public async Task<ActionResult<Product>> PostProduct([FromBody] ProductRequest request)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }

            var product = _productService.BuildProduct(request);
            
            await _productRepository.AddAsync(product);

            return CreatedAtAction(nameof(GetProduct), new { id = product.Id }, product);
        }
        
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _productRepository.GetProductAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            await _productRepository.DeleteAsync(product);

            return NoContent();
        }

        private bool ProductExists(int id)
        {
            return _productRepository.ProductExists(id);
        }
        
        // GET /api/products/search?searchTerm=&categoryId=&minPrice=&maxPrice=&inStock=&sortBy=&sortOrder=&pageNumber=&pageSize=
        [HttpGet("search")]
        public async Task<ActionResult<PagedResult<Product>>> Search(
            [FromQuery] string? searchTerm,
            [FromQuery] long? categoryId,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice,
            [FromQuery] bool? inStock,
            [FromQuery] string? sortBy,
            [FromQuery] string? sortOrder,
            [FromQuery] int pageNumber = 1,
            [FromQuery] int pageSize = 10)
        {
            var result = await _productRepository.SearchAsync(
                searchTerm,
                categoryId,
                minPrice,
                maxPrice,
                inStock,
                sortBy,
                sortOrder,
                pageNumber,
                pageSize);

            // Shape the response exactly as requested
            var response = new
            {
                items = result.Items,
                totalCount = result.TotalCount,
                pageNumber = result.PageNumber,
                pageSize = result.PageSize,
                totalPages = result.TotalPages
            };

            return Ok(response);
        }
    }
}
