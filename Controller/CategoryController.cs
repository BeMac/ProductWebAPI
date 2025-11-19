using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Models;
using ProductWebApi.Repositories;

namespace ProductWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ProductContext _context;
        private readonly IProductRepository _productRepository;

        public CategoryController(ProductContext context, IProductRepository productProductRepository)
        {
            _context = context;
            _productRepository = productProductRepository;
        }

        // GET: api/Category/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null) return NotFound();
            return category;
        }

        // POST: api/Category/1
        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (category == null)
                return BadRequest();

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }


        // DELETE: api/Products/4
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}

