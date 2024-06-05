using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Polly;
using ShippingServices.DAL;
using ShippingServices.DAL.Interfaces;
using ShippingServices.DTO;
using ShippingServices.Models;
using ShippingServices.Services;


var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//regist services
builder.Services.AddScoped<IShipping, ShippingDAL>();

//regist Order Detail Services
builder.Services.AddHttpClient<IOrderServices, OrderDetailServices>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(8000)));
builder.Services.AddHttpClient<IWalletServices, WalletServices>()
.AddTransientHttpErrorPolicy(p => p.WaitAndRetryAsync(3, _ => TimeSpan.FromMilliseconds(8000)));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();



app.MapGet("/api/shipping/", (IShipping shippingDAL) =>
{
    List<ShippingDTO> shippingDTO = new List<ShippingDTO>();
    var shipping = shippingDAL.GetAll();
    foreach (var shippings in shipping)
    {
        shippingDTO.Add(new ShippingDTO
        {
            // Populate DTO properties based on Product properties
            ShippingId = shippings.ShippingId,
            OrderDetailId = shippings.OrderDetailId,
            ShippingVendor = shippings.ShippingVendor,
            ShippingDate = shippings.ShippingDate,
            ShippingStatus = shippings.ShippingStatus,
            BeratBarang = shippings.BeratBarang,
            BiayaShipping = shippings.BiayaShipping,
        });
    }
    return Results.Ok(shippingDTO);
});
app.MapPut("/api/shipping/updateStatus", (IShipping shippingDAL, ShippingUpdateStatusDTO shippingUpdateStatusDTO) =>
{
    try
    {
        var shipping = new Shipping
        {
            ShippingId = shippingUpdateStatusDTO.ShippingId,
            ShippingStatus = shippingUpdateStatusDTO.ShippingStatus,
        };

        shippingDAL.UpdateStatus(shipping); // Call the Update method with the created Product object

        return Results.Ok();
    }
    catch (Exception ex)
    {
        return Results.BadRequest(ex.Message);
    }
});

app.MapGet("/api/shipping/estimatedDate", (int id, IShipping shippingDAL) =>
{
    DateOnly estimatedDate = shippingDAL.GetEstimatedDate(id); // Retrieve the estimated date
    DateOnly today = DateOnly.FromDateTime(DateTime.UtcNow); // Get today's date

    int daysDifference = estimatedDate.DayNumber - today.DayNumber; // Calculate the difference in days
    string responseMessage;

    if (daysDifference < 0)
    {
        responseMessage = "Your package should have arrived. If not, please contact customer support.";
    }
    else if (daysDifference == 0)
    {
        responseMessage = "Your package should arrive today.";
    }
    else
    {
        responseMessage = $"Your package should arrive in {daysDifference} {(daysDifference == 1 ? "day" : "days")}.";
    }

    return Results.Ok(responseMessage);
});



app.MapPost("/api/shipping/", async (IShipping shippingDAL, ShippingInsertDTO shippingInsertDTO, IOrderServices orderDetailServices, IWalletServices walletServices) =>
{
    try
    {
        var currentDate = DateTime.Today;
        if (shippingInsertDTO.ShippingDate.Date < currentDate)
        {
            return Results.BadRequest("Shipping Date cannot be before today's date");
        }
        var weight = shippingInsertDTO.BeratBarang;
        if (weight <= 0)
        {
            return Results.BadRequest("Berat Barang cannot be lower than 1 kg");
        }
        var price = shippingInsertDTO.BiayaShipping;
        if (price <= 1000)
        {
            return Results.BadRequest("Price cannot be lower than Rp. 1000");
        }

        // Get the order detail
        var order = await orderDetailServices.GetOrderById(shippingInsertDTO.OrderDetailId);
        if (order == null)
        {
            Console.WriteLine("Order ID not found");
            return Results.BadRequest("Order ID not found");
        }

        // Get the username associated with the order
        var orderUsername = order.username;

        // Get the wallet associated with the order's username
        var wallet = await walletServices.GetSaldo(orderUsername);
        if (wallet == null)
        {
            Console.WriteLine("Wallet not found for the order's username");
            return Results.BadRequest("Wallet not found for the order's username");
        }

        // Compare the username in order and wallet
        if (orderUsername != wallet.username)
        {
            Console.WriteLine("Mismatch between order username and wallet username");
            return Results.BadRequest("Mismatch between order username and wallet username");
        }

        // Deduct the saldo from the wallet
        wallet.saldo -= shippingInsertDTO.BiayaShipping; // Assuming BiayaShipping is the amount to be deducted

        // Update the wallet using the wallet service
        await walletServices.UpdateWalletBySaldo(new WalletUpdateSaldoDTO
        {
            Username = wallet.username, // Pass the username
            Saldo = wallet.saldo // Pass the updated saldo
        });

        Shipping shipping = new Shipping
        {
            OrderDetailId = shippingInsertDTO.OrderDetailId,
            ShippingVendor = shippingInsertDTO.ShippingVendor,
            ShippingDate = shippingInsertDTO.ShippingDate,
            ShippingStatus = shippingInsertDTO.ShippingStatus,
            BeratBarang = shippingInsertDTO.BeratBarang,
            BiayaShipping = shippingInsertDTO.BiayaShipping,
        };
        var ship = shippingDAL.Insert(shipping);

        // Return a success result
        return Results.Created($"/api/shipping/{shipping.ShippingId}", ship);

    }
    catch (Exception ex)
    {
        // Handle exceptions
        return Results.BadRequest(ex.Message);
    }
});


app.Run();

