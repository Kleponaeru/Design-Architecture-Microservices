using UserServices.DAL;
using UserServices.DAL.Interfaces;
using UserServices.DTO;
using UserServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//regist user services
builder.Services.AddScoped<IUser, UserDAL>();


var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/all/user/", (IUser userDAL) =>
{
    List<UserDTO> userDTO = new List<UserDTO>();
    var users = userDAL.GetAll();
    foreach (var user in users)
    {

        userDTO.Add(new UserDTO
        {
            UserId = user.UserId,
            Name = user.Name,
            Email = user.Email,

        });
    }
    return Results.Ok(userDTO);
});
app.MapPost("/api/add/user", (IUser userDAL, UserCreateDTO userCreateDTO) =>
{
    try
    {
        User user = new User
        {
            Name = userCreateDTO.Name,
            Email = userCreateDTO.Email,
        };

        var users = userDAL.Insert(user);

        // Return 201 Created with the created product
        return Results.Created($"/api/add/user/{users.UserId}", users);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});
app.MapGet("/api/find/user/{id}", (IUser userDAL, int id) =>
{
    UserDTO userDTO = new UserDTO();
    var user = userDAL.GetById(id);
    if (user == null)
    {
        return Results.NotFound();
    }
    userDTO.UserId = user.UserId;
    return Results.Ok(user);
});
app.MapDelete("/api/delete/user/{id}", (IUser userDAL, int id) =>
{
    try
    {
        userDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/api/update/user", (IUser userDAL, UserUpdateDTO userUpdateDTO) =>
{
    try
    {
        var user = new User
        {
            UserId = userUpdateDTO.UserId,
            Name = userUpdateDTO.Name,
            Email = userUpdateDTO.Email,
        };

        userDAL.Update(user); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

