using CatalogServices.DAL.Interfaces;
using CatalogServices.DTO;
using CatalogServices.Models;
using Microsoft.AspNetCore.Mvc;
using System.Data.SqlClient;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<ICategory, CategoryDAL>();
// builder.Services.AddScoped<IProduct, ProductDAL>();
builder.Services.AddScoped<ICategory, CategoryDapper>();
builder.Services.AddScoped<IProduct, ProductDapper>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


app.MapGet("/api/categories/", (ICategory categoryDAL) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDAL.GetAll();
    foreach (var category in categories)
    {

        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});
app.MapGet("/api/category/{name}", (ICategory categoryDAL, string categoryName) =>
{
    List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    var categories = categoryDAL.GetByName(categoryName);
    foreach (var category in categories)
    {
        categoriesDto.Add(new CategoryDTO
        {
            CategoryName = category.CategoryName
        });
    }
    return Results.Ok(categoriesDto);
});

app.MapGet("/api/categories/{id}", (ICategory categoryDAL, int id) =>
{
    CategoryDTO categoryDto = new CategoryDTO();
    var category = categoryDAL.GetById(id);
    if (category == null)
    {
        return Results.NotFound();
    }
    categoryDto.CategoryName = category.CategoryName;
    return Results.Ok(category);
});

app.MapPost("/api/categories", (ICategory categoryDAL, CategoryCreateDto categoryCreateDto) =>
{
    try
    {
        Category category = new Category
        {
            CategoryName = categoryCreateDto.CategoryName
        };
        categoryDAL.Insert(category);

        //return 201 Created
        return Results.Created($"/api/categories/{category.CategoryID}", category);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/api/categories", (ICategory categoryDAL, CategoryUpdateDto categoryUpdateDTO) =>
{
    try
    {
        var category = new Category
        {
            CategoryID = categoryUpdateDTO.CategoryID,
            CategoryName = categoryUpdateDTO.CategoryName
        };
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


app.MapGet("/api/products/", (IProduct productDAL) =>
{
    List<ProductDTO> productsDto = new List<ProductDTO>();
    var products = productDAL.GetAll();
    foreach (var product in products)
    {
        productsDto.Add(new ProductDTO
        {
            // Populate DTO properties based on Product properties
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity
        });
    }
    return Results.Ok(productsDto);
});
app.MapGet("/api/products/{productID}", (IProduct productDAL, int id) =>
{
    try
    {
        var product = productDAL.GetById(id);
        if (product == null)
        {
            return Results.NotFound();
        }

        var productDTO = new ProductDTO
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity
        };

        return Results.Ok(productDTO);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/products/category/{categoryID}", (IProduct productDAL, int id) =>
{
    try
    {
        var products = productDAL.GetByCategoryId(id);
        if (products == null || !products.Any())
        {
            return Results.NotFound();
        }

        var productDTOs = products.Select(product => new ProductDTO
        {
            Name = product.Name,
            Description = product.Description,
            Price = product.Price,
            Quantity = product.Quantity
            // Add other properties as needed
        }).ToList();

        return Results.Ok(productDTOs);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});




app.MapGet("/api/products/category/name", (IProduct productDAL, string categoryName) =>
{
    try
    {
        List<ProductDTO> productsDto = new List<ProductDTO>();
        var products = productDAL.GetByCategory(categoryName); // Call GetByCategory method with categoryName parameter
        foreach (var product in products)
        {
            productsDto.Add(new ProductDTO
            {
                // Populate DTO properties based on Product properties
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Quantity = product.Quantity
            });
        }
        return Results.Ok(productsDto);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.MapDelete("/api/products/{id}", (IProduct productDAL, int id) =>
{
    try
    {
        productDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/api/products", (IProduct productDAL, ProductCreateDto productCreateDto) =>
{
    try
    {
        Product product = new Product
        {
            Name = productCreateDto.Name,
            Description = productCreateDto.Description,
            Price = productCreateDto.Price,
            Quantity = productCreateDto.Quantity,
            CategoryID = productCreateDto.CategoryID
        };

        productDAL.Insert(product);

        // Return 201 Created with the created product
        return Results.Created($"/api/products/{product.ProductID}", product);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});

app.MapPut("/api/products", (IProduct productDAL, ProductUpdateDto productUpdateDto) =>
{
    try
    {
        var product = new Product
        {
            ProductID = productUpdateDto.ProductID,
            Name = productUpdateDto.Name,
            Description = productUpdateDto.Description,
            Price = productUpdateDto.Price,
            Quantity = productUpdateDto.Quantity,
            CategoryID = productUpdateDto.CategoryID
        };

        productDAL.Update(product); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});



app.Run();
