using ProductWebApi.Models;
using ProductWebApi.Repositories;

namespace ProductWebApi.Services;

public interface IProductService
{
    Product BuildProduct(ProductRequest request);
    
}

public class ProductService : IProductService
{
    private readonly IProductRepository _productRepository;
    private readonly ICategoryRepository _categoryRepository;

    public ProductService(IProductRepository productRepository, ICategoryRepository categoryRepository)
    {
        _productRepository = productRepository;
        _categoryRepository = categoryRepository;
    }


    public Product BuildProduct(ProductRequest request)
    {
        Category? category = _categoryRepository.GetById(request.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Invalid CategoryId");
        }

        var product = new Product
        {            
            Id = request.Id,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = category.Id,
            CreatedDate = DateTime.UtcNow,            
            IsActive = true
        };



        return product;
    }    
}