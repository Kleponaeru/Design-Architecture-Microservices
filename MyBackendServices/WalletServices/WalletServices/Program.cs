using WalletServices.DAL;
using WalletServices.DTO;
using WalletServices.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//regist services
builder.Services.AddScoped<IWallet, WalletDAL>();

var app = builder.Build();



//regist headers services
// builder.Services.AddHttpClient<IProductServices, ProductServices>()
// .AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(10000)));

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.MapGet("/api/user/", (IWallet walletDAL) =>
{
    List<WalletDTO> walletDTO = new List<WalletDTO>();
    var wallet = walletDAL.GetAll();
    foreach (var wallets in wallet)
    {
        walletDTO.Add(new WalletDTO
        {
            // Populate DTO properties based on Product properties
            WalletId = wallets.WalletId,
            Username = wallets.Username,
            Password = wallets.Password,
            FullName = wallets.FullName,
            Saldo = wallets.Saldo,
        });
    }
    return Results.Ok(walletDTO);
});


app.MapPost("/api/user", (IWallet walletDAL, WalletCreateDTO walletCreateDTO) =>
{
    try
    {
        Wallet wallet = new Wallet
        {

            Username = walletCreateDTO.Username,
            Password = walletCreateDTO.Password,
            FullName = walletCreateDTO.FullName,
            Saldo = walletCreateDTO.Saldo,
        };

        var insert = walletDAL.Insert(wallet);

        var responseObject = new
        {
            Username = wallet.Username,
            Password = wallet.Password,
            FullName = wallet.FullName,
            Saldo = wallet.Saldo,
        };

        // Return 201 Created with the created product
        return Results.Created($"/api/wallet/{insert.Username}", responseObject);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/wallet/{username}", (IWallet walletDAL, string username) =>
{
    try
    {
        var wallets = walletDAL.GetByUsername(username);

        if (wallets == null)
        {
            return Results.NotFound();
        }

        var walletData = new WalletDTO
        {
            WalletId = wallets.WalletId,
            Username = wallets.Username,
            Password = wallets.Password,
            FullName = wallets.FullName,
            Saldo = wallets.Saldo,
        };

        return Results.Ok(walletData);
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }


});



app.MapDelete("/api/user/{id}", (IWallet walletDAL, int id) =>
{
    try
    {
        walletDAL.Delete(id);
        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});
app.MapPost("/api/wallet/topUp", (IWallet walletDAL, WalletTopUpDTO walletTopUpDTO) =>
{
    try
    {
        Wallet wallet = new Wallet
        {
            WalletId = walletTopUpDTO.WalletId,
            Saldo = walletTopUpDTO.Saldo,
        };

        var updatedWallet = walletDAL.TopUp(wallet);

        var responseObject = new
        {
            WalletId = updatedWallet.WalletId,
            Saldo = updatedWallet.Saldo,
            Username = updatedWallet.Username,
            FullName = updatedWallet.FullName
        };

        // Return 200 OK with the updated wallet
        return Results.Ok(responseObject);
    }
    catch (Exception ex)
    {
        // Return 400 Bad Request if there's an error
        return Results.BadRequest(ex.Message);
    }
});
app.MapPut("/api/wallet/updateAfterOrder", (IWallet walletDAL, WalletUpdateSaldoDTO walletUpdateSaldoDTO) =>
{
    try
    {
        if (string.IsNullOrWhiteSpace(walletUpdateSaldoDTO.Username))
        {
            return Results.BadRequest("Username cannot be null or empty.");
        }

        walletDAL.UpdateSaldoAfterOrder(walletUpdateSaldoDTO);

        // Fetch the updated wallet to return in the response
        var updatedWallet = walletDAL.GetByUsername(walletUpdateSaldoDTO.Username);
        if (updatedWallet == null)
        {
            return Results.NotFound("Wallet not found.");
        }

        return Results.Ok(updatedWallet);
    }
    catch (Exception ex)
    {
        return Results.Problem(ex.Message);
    }
});



app.Run();

