using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Models;

namespace ProductWebApi.Repositories;

public interface IProductRepository
{
    Task<Product?> GetProductAsync(long id);
    Task<IEnumerable<Product>> GetAllAsync();
    Task AddAsync(Product product);
    Task<Product?> UpdateAsync(int id, Product product);
    Task DeleteAsync(Product product);
    bool ProductExists(int id);
    Task<PagedResult<Product>> SearchAsync(
        string? searchTerm,
        long? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? inStock,
        string? sortBy,
        string? sortOrder,
        int pageNumber,
        int pageSize);
}

public class ProductRepository : IProductRepository
{
    private readonly ProductContext _context;

    public ProductRepository(ProductContext context)
    {
        _context = context;
    }

    public async Task<Product?> GetProductAsync(long id)
    {
        return await _context.Products
            .AsNoTracking()
            .FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task<IEnumerable<Product>> GetAllAsync()
    {
        return await _context.Products
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task AddAsync(Product product)
    {
        _context.Products.Add(product);        
        await _context.SaveChangesAsync();
    }
    
    public async Task<Product?> UpdateAsync(int id, Product product)
    {
        if (id != product.Id)
        {
            return null;
        }

        var existingProduct = await _context.Products.FindAsync(id);
        if (existingProduct == null)
        {
            return null;
        }

        // Update properties
        existingProduct.Name = product.Name;
        existingProduct.Description = product.Description;
        existingProduct.Price = product.Price;
        existingProduct.StockQuantity = product.StockQuantity;
        existingProduct.CategoryId = product.CategoryId;
        existingProduct.IsActive = product.IsActive;
        // Don't update CreatedDate

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!await ProductExistsAsync(id))
            {
                return null;
            }
            throw;
        }

        return existingProduct;
    }

    private async Task<bool> ProductExistsAsync(int id)
    {
        return await _context.Products.AnyAsync(p => p.Id == id);
    }
    
    public async Task DeleteAsync(Product product)
    {
        _context.Products.Remove(product);
        await _context.SaveChangesAsync();
    }
    
    public bool ProductExists(int id)
    {
        return _context.Products.Any(e => e.Id == id);
    }
    
    public async Task<PagedResult<Product>> SearchAsync(
        string? searchTerm,
        long? categoryId,
        decimal? minPrice,
        decimal? maxPrice,
        bool? inStock,
        string? sortBy,
        string? sortOrder,
        int pageNumber,
        int pageSize)
    {
        if (pageNumber <= 0) pageNumber = 1;
        if (pageSize <= 0) pageSize = 10;
        if (pageSize > 100) pageSize = 100;

        // Base query: only active products
        IQueryable<Product> query = _context.Products
            .AsNoTracking()
            .Where(p => p.IsActive);

        // Search term across Name + Description, case-insensitive,
        // AND logic for multiple words
        if (!string.IsNullOrWhiteSpace(searchTerm))
        {
            var words = searchTerm
                .Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries)
                .Select(w => w.ToLowerInvariant())
                .ToArray();

            foreach (var word in words)
            {
                var w = word;
                query = query.Where(p =>
                    (!string.IsNullOrEmpty(p.Name) && p.Name.ToLower().Contains(w)) ||
                    (!string.IsNullOrEmpty(p.Description) && p.Description.ToLower().Contains(w)));
            }
        }

        // Optional filters
        if (categoryId.HasValue)
        {
            query = query.Where(p => p.CategoryId == categoryId.Value);
        }

        if (minPrice.HasValue)
        {
            query = query.Where(p => p.Price >= minPrice.Value);
        }

        if (maxPrice.HasValue)
        {
            query = query.Where(p => p.Price <= maxPrice.Value);
        }

        if (inStock.HasValue)
        {
            query = query.Where(p => p.StockQuantity > 0);
        }

        // Sorting
        bool descending = string.Equals(sortOrder, "desc", StringComparison.OrdinalIgnoreCase);

        query = (sortBy ?? string.Empty).ToLower() switch
        {
            "name" => descending
                ? query.OrderByDescending(p => p.Name).ThenBy(p => p.Id)
                : query.OrderBy(p => p.Name).ThenBy(p => p.Id),

            "price" => descending
                ? query.OrderByDescending(p => p.Price).ThenBy(p => p.Id)
                : query.OrderBy(p => p.Price).ThenBy(p => p.Id),

            "category" or "categoryid" => descending
                ? query.OrderByDescending(p => p.CategoryId).ThenBy(p => p.Id)
                : query.OrderBy(p => p.CategoryId).ThenBy(p => p.Id),

            _ => descending
                ? query.OrderByDescending(p => p.Id)
                : query.OrderBy(p => p.Id)
        };

        // Total count & page data (filters applied in DB)
        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((pageNumber - 1) * pageSize)
            .Take(pageSize)
            .ToListAsync();

        var totalPages = (int)Math.Ceiling(totalCount / (double)pageSize);

        return new PagedResult<Product>
        {
            Items = items,
            TotalCount = totalCount,
            PageNumber = pageNumber,
            PageSize = pageSize,
            TotalPages = totalPages
        };
    }
}