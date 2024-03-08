using CatalogServices.DAL.Interfaces;
using CatalogServices.Models;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategory, CategoryDAL>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

var products = new List<Product>
{
   new Product { ProductId = 1, Name ="Apple", Description= "Regular Apple", Price= 10101, CategoryID= 1, Quantity = 12},
   new Product { ProductId = 2, Name ="Orange", Description= "Regular Orange", Price= 10103, CategoryID= 1, Quantity = 20},
   new Product { ProductId = 3, Name ="Pear", Description= "Regular Pear", Price= 10102, CategoryID= 1, Quantity = 10},
   new Product { ProductId = 4, Name ="Pineapple", Description= "Regular Pineapple", Price= 10102, CategoryID= 1, Quantity = 101},
};

app.MapGet("/api/categories/", (ICategory categoryDAL) =>
{
    var categories = categoryDAL.GetAll();
    return Results.Ok(categories);
});
app.MapGet("/api/category/{name}", (ICategory categoryDAL, string categoryName) =>
{
    var categories = categoryDAL.GetByName(categoryName);
    return Results.Ok(categories);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDAL, int id) =>
{
    var category = categoryDAL.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    return Results.Ok(category);
});

app.MapPost("/api/categories", (ICategory categoryDAL, Category category) =>
{
    try
    {
        categoryDAL.Insert(category);
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/api/categories", (ICategory categoryDAL, Category category) =>
{
    try
    {
        categoryDAL.Update(category);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapDelete("/api/categories/{id}", (ICategory categoryDAL, int id) =>
{
     try
    {
        categoryDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.Run();

record WeatherForecast(DateOnly Date, int TemperatureC, string? Summary)
{
    public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);
}
