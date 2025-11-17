using Microsoft.EntityFrameworkCore;
using ProductWebApi.Models;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddOpenApi();
//builder.Services.AddDbContext<ProductContext>(opt => opt.UseInMemoryDatabase("Product"));

var conn = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ProductContext>(options =>
    options.UseNpgsql(conn));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.MapControllers();
app.Run();
