using ProductWebApi.Models;
using ProductWebApi.Repositories;

namespace ProductWebApi.Services;

public interface IProductService
{
    Product BuildProduct(CreateProductRequest request);
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


    public Product BuildProduct(CreateProductRequest request)
    {
        Category? category = _categoryRepository.GetById(request.CategoryId);
        if (category == null)
        {
            throw new ArgumentException("Invalid CategoryId");
        }

        var product = new Product
        {
            Id = request.CategoryId,
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            StockQuantity = request.StockQuantity,
            CategoryId = category.Id,
            CreatedDate = DateTime.UtcNow,
            Category = category,
            IsActive = true
        };



        return product;
    }    
}