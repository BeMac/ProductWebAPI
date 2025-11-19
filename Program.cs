using Microsoft.EntityFrameworkCore;
using ProductWebApi.Data;
using ProductWebApi.Repositories;
using ProductWebApi.Services;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddOpenApi();
builder.Services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("Product"));
builder.Services.AddDbContext<CategoryContext>(opt => opt.UseInMemoryDatabase("Product")); // register CategoryContext


//DI Repositories
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();

//DI Services
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

//swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();
app.UseSwagger();
app.MapControllers();
app.Run();