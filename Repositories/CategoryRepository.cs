using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Models;

namespace ProductWebApi.Repositories;

public interface ICategoryRepository
{
    IQueryable<Category> GetAll();
    Category? GetById(int categoryId);
}

public class CategoryRepository : ICategoryRepository
{
    private readonly CategoryContext _context;
    public CategoryRepository(CategoryContext context)
    {
        _context = context;
    }
    public IQueryable<Category> GetAll()
    {
        return _context.Categories.AsNoTracking();
    }
    public Category? GetById(int categoryId)
    {
        return _context.Categories
            .AsNoTracking()
            .FirstOrDefault(c => c.Id == categoryId);
    }
}