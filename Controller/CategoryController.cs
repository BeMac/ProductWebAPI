using System.Net;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ProductWebApi.Models;
using ProductWebApi.Repositories;

namespace ProductWebApi.Controller
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;

        public CategoryController(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }
        
        [HttpGet]
        public async Task<ActionResult<Category>> GetCategories()
        {
            return Ok(await _categoryRepository.GetAllAsync());
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _categoryRepository.GetCategoryAsync(id);

            if (category == null)
            {
                return NotFound();
            }

            return category;
        }

        [HttpPost]
        public async Task<ActionResult<Category>> PostCategory(Category category)
        {
            if (!ModelState.IsValid)
            {
                return ValidationProblem(ModelState);
            }
                
            await _categoryRepository.AddAsync(category);            
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }
    }
}

