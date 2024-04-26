using OrderServices.DAL.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/categories/", (ICustomer customerDAL) =>
{
    // List<CategoryDTO> categoriesDto = new List<CategoryDTO>();
    // var categories = categoryDAL.GetAll();
    // foreach (var category in categories)
    // {

    //     categoriesDto.Add(new CategoryDTO
    //     {
    //         CategoryName = category.CategoryName
    //     });
    // }
    // return Results.Ok(categoriesDto);
});



app.Run();
