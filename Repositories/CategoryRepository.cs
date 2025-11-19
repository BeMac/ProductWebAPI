using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Models;

namespace ProductWebApi.Repositories;

public interface ICategoryRepository
{
    Task<IEnumerable<Category>> GetAllAsync();
    Task<Category?> GetCategoryAsync(int id);
    Category GetCategory(int id);
    Task AddAsync(Category category);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryContext _context;
    
    public CategoryRepository(CategoryContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Category>> GetAllAsync()
    {
        return await _context.Categories
            .AsNoTracking()
            .ToListAsync();
    }
    
    public async Task AddAsync(Category category)
    {
        _context.Categories.Add(category);        
        await _context.SaveChangesAsync();
    }
    
    public async Task<Category?> GetCategoryAsync(int id)
    {
        return await _context.Categories
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);        
    }
    
    public Category GetCategory(int id)
    {
        return _context.Categories
            .AsNoTracking()
            .FirstOrDefault<Category?>(p => p.Id == id);        
    }
}