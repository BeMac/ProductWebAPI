
using Microsoft.EntityFrameworkCore;

namespace ProductWebApi.Models;

public class ProductContext : DbContext
{
    public ProductContext(DbContextOptions<ProductContext> options)
        : base(options)
    {
    }

    public DbSet<Product> Products { get; set; } = null!;
}