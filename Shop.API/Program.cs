using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddDbContext<Shop.Infrastructure.DatabaseContext>(options => options.UseInMemoryDatabase("eShop"));

builder.Services.AddScoped<Shop.Infrastructure.Interfaces.IOrderRepository, Shop.Infrastructure.OrderRepository>();
builder.Services.AddScoped<Shop.Infrastructure.Interfaces.IProductRepository, Shop.Infrastructure.ProductRepository>();
builder.Services.AddScoped<Shop.API.Services.Interfaces.IOrderService, Shop.API.Services.OrderService>();
builder.Services.AddScoped<Shop.API.Services.Interfaces.IProductService, Shop.API.Services.ProductService>();




var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
